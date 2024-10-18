using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración Ocelot
builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot();

// Agregar autenticación con JWT
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OaroeJ9f9Y4ChH8foAgZjGPUi1PbCeNR"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsRule", rule =>
    {
        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
    });
});

var app = builder.Build();
app.UseCors("CorsRule");

// Configuración Ocelot
await app.UseOcelot();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
