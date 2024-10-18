using Servicios.Api.Seguridad.Core.Entities;

namespace Servicios.Api.Seguridad.Core.JwtLogic
{
    public interface IJwtGenerator
    {
        string CreateToken(Usuario usuario);
    }
}
