using BTGPactualAPI.Models;
using BTGPactualAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BTGPactualAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class TransaccionesController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public TransaccionesController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<Transaccion>> Get() =>
            await _mongoDBService.GetTransaccionesAsync();
    }
}
