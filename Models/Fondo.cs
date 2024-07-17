using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BTGPactualAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Fondo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public decimal MontoMinimo { get; set; }
        public string Categoria { get; set; } = null!;
        // Agregar el nuevo campo para el monto inicial
        public decimal MontoInicial { get; set; }

    }
}
