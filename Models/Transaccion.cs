using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BTGPactualAPI.Models
{
    public class Transaccion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; }

        [BsonElement("tipo")]
        public string Tipo { get; set; } = null!;

        [BsonElement("monto")]
        public decimal Monto { get; set; }

        [BsonElement("fondoId")]
        public string FondoId { get; set; } = null!;
    }
}
