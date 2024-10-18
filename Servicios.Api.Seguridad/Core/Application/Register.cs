using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Servicios.Api.Seguridad.Core.Dto;
using Servicios.Api.Seguridad.Core.Entities;
using Servicios.Api.Seguridad.Core.JwtLogic;
using Servicios.Api.Seguridad.Core.Persistence;

namespace Servicios.Api.Seguridad.Core.Application
{
    public class Register
    {
        public class UsuarioRegisterCommand : IRequest<UsuarioDto>
        {
            public string Nombre { get; set; }

            public string Apellido { get; set; }

            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class UsuarioRegisterValidation : AbstractValidator<UsuarioRegisterCommand>
        {
            public UsuarioRegisterValidation()
            {
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre del usuario es obligatorio");
                RuleFor(x => x.Apellido).NotEmpty().WithMessage("El apellido del usuario es obligatorio");
                RuleFor(x => x.Username).NotEmpty().WithMessage("El username del usuario es obligatorio");
                RuleFor(x => x.Email).NotEmpty().WithMessage("El email del usuario es obligatorio");
                RuleFor(x => x.Password).NotEmpty().WithMessage("El password del usuario es obligatorio");
            }
        }

        public class UsuarioRegisterHadler : IRequestHandler<UsuarioRegisterCommand, UsuarioDto>
        {
            private readonly SeguridadContexto _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IMapper _mapper;
            private readonly IJwtGenerator _jwtGenerator;

            public UsuarioRegisterHadler(SeguridadContexto context, UserManager<Usuario> userManager, IMapper mapper, IJwtGenerator jwtGenerator)
            {
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<UsuarioDto> Handle(UsuarioRegisterCommand request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();

                if (existe)
                {
                    throw new Exception("El email del usuario ya se encuentra registrado");
                }

                existe = await _context.Users.Where(x => x.UserName == request.Username).AnyAsync();

                if (existe)
                {
                    throw new Exception("El username del usuario ya se encuentra registrado");
                }

                var usuario = new Usuario
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Email = request.Email,
                    UserName = request.Username
                };

                var resultado = await _userManager.CreateAsync(usuario, request.Password);

                if (resultado.Succeeded)
                {
                    var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
                    usuarioDto.Token = _jwtGenerator.CreateToken(usuario);
                    return usuarioDto;
                }
                throw new Exception(resultado.Errors.First().Description);
            }
        }
    }
}
