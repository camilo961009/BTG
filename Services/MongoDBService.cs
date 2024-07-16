using BTGPactualAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BTGPactualAPI.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Fondo> _fondosCollection;
        private readonly IMongoCollection<Transaccion> _transaccionesCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);

            _fondosCollection = mongoDatabase.GetCollection<Fondo>(mongoDBSettings.Value.FondosCollectionName);
            _transaccionesCollection = mongoDatabase.GetCollection<Transaccion>(mongoDBSettings.Value.TransaccionesCollectionName);
        }

        public async Task<List<Fondo>> GetFondosAsync() =>
            await _fondosCollection.Find(_ => true).ToListAsync();

        public async Task CreateFondoAsync(Fondo nuevoFondo) =>
            await _fondosCollection.InsertOneAsync(nuevoFondo);

        public async Task<Fondo?> GetFondoByIdAsync(string id) =>
            await _fondosCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task RemoveFondoAsync(string id) =>
            await _fondosCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Transaccion>> GetTransaccionesAsync() =>
            await _transaccionesCollection.Find(_ => true).ToListAsync();

        public async Task CreateTransaccionAsync(Transaccion nuevaTransaccion) =>
            await _transaccionesCollection.InsertOneAsync(nuevaTransaccion);
    }
}
