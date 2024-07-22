using BackEndAPIFondosBTG.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BackEndAPIFondosBTG.Services
{
    public class MongoDBService
    {
        
        private readonly IMongoCollection<Fund> _fondosCollection;
        private readonly IMongoCollection<Transaction> _transaccionesCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);

            _fondosCollection = mongoDatabase.GetCollection<Fund>(mongoDBSettings.Value.FondosCollectionName);
            _transaccionesCollection = mongoDatabase.GetCollection<Transaction>(mongoDBSettings.Value.TransaccionesCollectionName);
        }

        public async Task<List<Fund>> GetFondosAsync() =>
            await _fondosCollection.Find(_ => true).ToListAsync();

        public async Task CreateFondoAsync(Fund nuevoFondo) =>
            await _fondosCollection.InsertOneAsync(nuevoFondo);

        public async Task<Fund?> GetFondoByIdAsync(string id) =>
            await _fondosCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task RemoveFondoAsync(string id) =>
            await _fondosCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Transaction>> GetTransaccionesAsync() =>
            await _transaccionesCollection.Find(_ => true).ToListAsync();

        public async Task CreateTransaccionAsync(Transaction nuevaTransaccion) =>
            await _transaccionesCollection.InsertOneAsync(nuevaTransaccion);
    }
}
