﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Servicios.Api.Seguridad.Core.Application;
using Servicios.Api.Seguridad.Core.Dto;

namespace Servicios.Api.Seguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDto>> Registrar(Register.UsuarioRegisterCommand parametros)
        {
            return await _mediator.Send(parametros);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login(Login.UsuarioLoginCommand parametros)
        {
            return await _mediator.Send(parametros);
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioDto>> Get()
        {
            return await _mediator.Send(new UsuarioActual.UsuarioActualCommand());
        }
    }
}
