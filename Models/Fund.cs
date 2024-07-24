using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackEndAPIFondosBTG.Models
{
    public class Fund
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;

        [BsonElement("nombre")]
        public string nombre { get; set; } = null!;

        [BsonElement("monto_vinculacion_fondo")]
        public decimal montoVinculacionFondo { get; set; }
       
        [BsonElement("categoria")]
        public string categoria { get; set; } = null!;

        [BsonElement("monto_inicial")] 
        public decimal montoInicial { get; set; }

        [BsonElement("correo")] 
        public string correo { get; set; } = null!;

        [BsonElement("celular")] 
        public string celular { get; set; } = null!;
    }
}
