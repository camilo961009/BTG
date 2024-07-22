using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackEndAPIFondosBTG.Models
{
    public class Client
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public decimal Saldo { get; set; }
    }
}
