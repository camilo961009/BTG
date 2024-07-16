using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BTGPactualAPI.Models
{
    public class Transaccion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; }

        [BsonElement("tipo")]
        public string Tipo { get; set; }

        [BsonElement("monto")]
        public decimal Monto { get; set; }

        [BsonElement("fondoId")]
        public string FondoId { get; set; }
    }
}
