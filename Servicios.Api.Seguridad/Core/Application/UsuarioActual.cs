using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicios.Api.Seguridad.Core.Dto;
using Servicios.Api.Seguridad.Core.Entities;
using Servicios.Api.Seguridad.Core.JwtLogic;

namespace Servicios.Api.Seguridad.Core.Application
{
    public class UsuarioActual
    {
        public class UsuarioActualCommand : IRequest<UsuarioDto>
        {
        }

        public class UsuarioActualHandler : IRequestHandler<UsuarioActualCommand, UsuarioDto>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly IUsuarioSesion _usuarioSesion;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IMapper _mapper;

            public UsuarioActualHandler(UserManager<Usuario> userManager, IUsuarioSesion usuarioSesion, IJwtGenerator jwtGenerator, IMapper mapper)
            {
                _userManager = userManager;
                _usuarioSesion = usuarioSesion;
                _jwtGenerator = jwtGenerator;
                _mapper = mapper;
            }

            public async Task<UsuarioDto> Handle(UsuarioActualCommand request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.GetUsuarioSesion());

                if (usuario != null)
                {
                    var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
                    usuarioDto.Token = _jwtGenerator.CreateToken(usuario);
                    return usuarioDto;
                }
                throw new Exception("Usuario no encontrado");
            }
        }
    }
}
