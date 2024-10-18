﻿using Microsoft.IdentityModel.Tokens;
using Servicios.Api.Seguridad.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Servicios.Api.Seguridad.Core.JwtLogic
{
    public class JwtGenerator : IJwtGenerator
    {
        public string CreateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim("username", usuario.UserName),
                new Claim("nombre", usuario.Nombre),
                new Claim("apellido", usuario.Apellido)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OaroeJ9f9Y4ChH8foAgZjGPUi1PbCeNR"));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(3),
                SigningCredentials = credential
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}
