using AutoMapper;
using Servicios.Api.Seguridad.Core.Entities;

namespace Servicios.Api.Seguridad.Core.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioDto>();
        }
    }
}
