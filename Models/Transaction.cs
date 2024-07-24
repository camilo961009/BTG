using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackEndAPIFondosBTG.Models
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("fecha")]
        public DateTime fecha { get; set; }

        [BsonElement("tipo")]
        public string tipo { get; set; } = null!;

        [BsonElement("monto")]
        public decimal monto { get; set; }

        [BsonElement("fondoId")]
        public string FondoId { get; set; } = null!;
    }
}
