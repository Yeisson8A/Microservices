using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicios.Api.Seguridad.Core.Dto;
using Servicios.Api.Seguridad.Core.Entities;
using Servicios.Api.Seguridad.Core.JwtLogic;
using Servicios.Api.Seguridad.Core.Persistence;

namespace Servicios.Api.Seguridad.Core.Application
{
    public class Login
    {
        public class UsuarioLoginCommand : IRequest<UsuarioDto>
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class UsuarioLoginValidation : AbstractValidator<UsuarioLoginCommand>
        {
            public UsuarioLoginValidation()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("El email es obligatorio");
                RuleFor(x => x.Password).NotEmpty().WithMessage("El password es obligatorio");
            }
        }

        public class UsuarioLoginHadler : IRequestHandler<UsuarioLoginCommand, UsuarioDto>
        {
            private readonly SeguridadContexto _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IMapper _mapper;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly SignInManager<Usuario> _signInManager;

            public UsuarioLoginHadler(SeguridadContexto context, UserManager<Usuario> userManager, IMapper mapper, IJwtGenerator jwtGenerator, SignInManager<Usuario> signInManager)
            {
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
                _jwtGenerator = jwtGenerator;
                _signInManager = signInManager;
            }

            public async Task<UsuarioDto> Handle(UsuarioLoginCommand request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);

                if (usuario == null)
                {
                    throw new Exception("El email ingresado no se encuentra registrado");
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                if (resultado.Succeeded)
                {
                    var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
                    usuarioDto.Token = _jwtGenerator.CreateToken(usuario);
                    return usuarioDto;
                }
                throw new Exception("El email y/o password son incorrectos");
            }
        }
    }
}
