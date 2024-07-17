using BTGPactualAPI.Models;
using BTGPactualAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BTGPactualAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class FondosController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        private decimal montoInicial = 500000; // Monto inicial en COP

        public FondosController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<Fondo>> Get() =>
            await _mongoDBService.GetFondosAsync();

        [HttpPost]
        public async Task<IActionResult> Post(Fondo nuevoFondo)
        {
            if (montoInicial < nuevoFondo.MontoMinimo)
            {
                return BadRequest($"No tiene saldo disponible para vincularse al fondo {nuevoFondo.Nombre}");
            }

            montoInicial -= nuevoFondo.MontoMinimo;
            nuevoFondo.MontoInicial = montoInicial;


            // Crear el fondo en la base de datos
            await _mongoDBService.CreateFondoAsync(nuevoFondo);

            var transaccion = new Transaccion
            {
                Fecha = DateTime.Now,
                Tipo = "Creación",
                Monto = nuevoFondo.MontoMinimo,
                FondoId = nuevoFondo.Id
            };
            await _mongoDBService.CreateTransaccionAsync(transaccion);
     

            return CreatedAtAction(nameof(Get), new { id = nuevoFondo.Id }, nuevoFondo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var fondo = await _mongoDBService.GetFondoByIdAsync(id);
            if (fondo == null)
            {
                return NotFound();
            }

            // Sumar el MontoMinimo al monto inicial al eliminar un fondo
            montoInicial += fondo.MontoMinimo;

            // Actualizar el MontoInicial en el fondo eliminado
            fondo.MontoInicial = montoInicial;

            await _mongoDBService.RemoveFondoAsync(id);
            var transaccion = new Transaccion
            {
                Fecha = DateTime.Now,
                Tipo = "Eliminación",
                Monto = fondo.MontoMinimo,
                FondoId = id
            };
            await _mongoDBService.CreateTransaccionAsync(transaccion);
            return NoContent();
        }
    }
}

