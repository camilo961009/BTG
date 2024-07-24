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
        public async Task<List<Fund>> GetFund() =>
            await _mongoDBService.GetFondosAsync();

        [HttpPost]
        public async Task<IActionResult> createFund(Fund nuevoFondo)
        {
            if (montoInicial < nuevoFondo.montoVinculacionFondo)
            {
                return BadRequest($"No tiene saldo disponible para vincularse al fondo {nuevoFondo.nombre}");
            }

            montoInicial -= nuevoFondo.montoVinculacionFondo;
            nuevoFondo.montoInicial = montoInicial;

            await _mongoDBService.CreateFondoAsync(nuevoFondo);
            await RegistrarTransaccionAsync("Creación", nuevoFondo.montoVinculacionFondo, nuevoFondo.Id);

            if (!string.IsNullOrEmpty(nuevoFondo.celular))
            {
                var response = await _vonageClient.SmsClient.SendAnSmsAsync(new SendSmsRequest
                {
                    To = "57" + nuevoFondo.celular,
                    From = "YourAppName",
                    Text = $"Se ha creado un nuevo fondo: {nuevoFondo.nombre}. monto: {nuevoFondo.montoVinculacionFondo}"
                });
            }
            else
            {
                await _emailService.SendEmailAsync(nuevoFondo.correo, "Nuevo fondo creado",
                                               $"Se ha creado un nuevo fondo: {nuevoFondo.nombre}. monto: {nuevoFondo.montoVinculacionFondo}");
            }

            return CreatedAtAction(nameof(GetFund), new { id = nuevoFondo.Id }, nuevoFondo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteFund(string id)
        {
            var fondo = await _mongoDBService.GetFondoByIdAsync(id);
            if (fondo == null)
            {
                return NotFound();
            }

            montoInicial += fondo.montoVinculacionFondo;
            fondo.montoInicial = montoInicial;

            await _mongoDBService.RemoveFondoAsync(id);
            await RegistrarTransaccionAsync("Eliminación", fondo.montoVinculacionFondo, id);

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
                    fecha = DateTime.Now,
                    tipo = tipo,
                    monto = monto,
                    FondoId = fondoId
                };
                await _mongoDBService.CreateTransaccionAsync(transaccion);
            }
        }
    }
}