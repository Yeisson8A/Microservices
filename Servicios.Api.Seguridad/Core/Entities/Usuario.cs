using Microsoft.AspNetCore.Identity;

namespace Servicios.Api.Seguridad.Core.Entities
{
    public class Usuario : IdentityUser
    {
        public string Nombre { get; set; }

        public string Apellido { get; set; }
    }
}
