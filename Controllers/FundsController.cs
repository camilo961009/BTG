using BackEndAPIFondosBTG.Models;
using BackEndAPIFondosBTG.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Vonage.Messaging;
using Vonage;
using Vonage.Request;

namespace BackEndAPIFondosBTG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class FundsController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;
        private readonly EmailService _emailService;
        private readonly VonageClient _vonageClient;
        private decimal montoInicial = 500000;

        private readonly Dictionary<string, decimal> MontoMinimoPorFondo = new Dictionary<string, decimal>
    {
        { "FPV_BTG_PACTUAL_RECAUDADORA", 75000 },
        { "FPV_BTG_PACTUAL_ECOPETROL", 125000 },
        { "DEUDAPRIVADA", 50000 },
        { "FDO-ACCIONES", 250000 },
        { "FPV_BTG_PACTUAL_DINAMICA", 100000 }
    };

        public FundsController(MongoDBService mongoDBService, EmailService emailService)
        {
            _mongoDBService = mongoDBService;
            _emailService = emailService;
            var credentials = Credentials.FromApiKeyAndSecret("a66dc43e", "HT3DwTBsBVTcdzdV");
            _vonageClient = new VonageClient(credentials);
        }

        [HttpGet]
        public async Task<List<Fund>> Get() =>
            await _mongoDBService.GetFondosAsync();

        [HttpPost]
        public async Task<IActionResult> Post(Fund nuevoFondo)
        {
            if (montoInicial < nuevoFondo.MontoMinimo)
            {
                return BadRequest($"No tiene saldo disponible para vincularse al fondo {nuevoFondo.Nombre}");
            }

            montoInicial -= nuevoFondo.MontoMinimo;
            nuevoFondo.MontoInicial = montoInicial;

            await _mongoDBService.CreateFondoAsync(nuevoFondo);
            await RegistrarTransaccionAsync("Creación", nuevoFondo.MontoMinimo, nuevoFondo.Id);

            if (!string.IsNullOrEmpty(nuevoFondo.Celular))
            {
                var response = await _vonageClient.SmsClient.SendAnSmsAsync(new SendSmsRequest
                {
                    To = "57" + nuevoFondo.Celular,
                    From = "YourAppName",
                    Text = $"Se ha creado un nuevo fondo: {nuevoFondo.Nombre}. Monto: {nuevoFondo.MontoMinimo}"
                });
            }
            else
            {
                await _emailService.SendEmailAsync(nuevoFondo.Correo, "Nuevo fondo creado",
                                               $"Se ha creado un nuevo fondo: {nuevoFondo.Nombre}. Monto: {nuevoFondo.MontoMinimo}");
            }
 

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

            montoInicial += fondo.MontoMinimo;
            fondo.MontoInicial = montoInicial;

            await _mongoDBService.RemoveFondoAsync(id);
            await RegistrarTransaccionAsync("Eliminación", fondo.MontoMinimo, id);

            return NoContent();
        }

        private async Task RegistrarTransaccionAsync(string tipo, decimal monto, string fondoId)
        {
            if (string.IsNullOrEmpty(fondoId))
            {
                throw new ArgumentNullException(nameof(fondoId));
            }
            else
            {
                var transaccion = new Transaction
                {
                    Fecha = DateTime.Now,
                    Tipo = tipo,
                    Monto = monto,
                    FondoId = fondoId
                };
                await _mongoDBService.CreateTransaccionAsync(transaccion);
            }
        }
    }
}