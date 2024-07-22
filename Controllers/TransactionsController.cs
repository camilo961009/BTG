using BackEndAPIFondosBTG.Models;
using BackEndAPIFondosBTG.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPIFondosBTG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class TransactionsController : ControllerBase
    {

        private readonly MongoDBService _mongoDBService;

        public TransactionsController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<Transaction>> Get() =>
            await _mongoDBService.GetTransaccionesAsync();
    }
}
