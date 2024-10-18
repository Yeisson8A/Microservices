using MongoDB.Bson.Serialization.Attributes;

namespace Servicios.Api.Libreria.Core.Entities
{
    [BsonCollection("Autor")]
    public class AutorEntity : Document
    {
        [BsonElement("nombre")]
        public string? Nombre { get; set; }

        [BsonElement("apellido")]
        public string? Apellido { get; set; }

        [BsonElement("gradoAcademico")]
        public string? GradoAcademico { get; set; }

        [BsonElement("nombreCompleto")]
        public string? NombreCompleto { get; set; }
    }
}
