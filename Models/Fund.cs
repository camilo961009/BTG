using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackEndAPIFondosBTG.Models
{
    public class Fund
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public decimal MontoMinimo { get; set; }
        public string Categoria { get; set; } = null!;
        public decimal MontoInicial { get; set; }
        public string Correo { get; set; } = null!;
        public string Celular { get; set; } = null!;
    }
}
