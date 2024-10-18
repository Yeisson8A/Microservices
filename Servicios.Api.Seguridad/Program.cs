using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Servicios.Api.Seguridad.Core.Application;
using Servicios.Api.Seguridad.Core.Entities;
using Servicios.Api.Seguridad.Core.JwtLogic;
using Servicios.Api.Seguridad.Core.Persistence;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SeguridadContexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionDB"));
});

// Configuración uso Core Identity
var objectBuilder = builder.Services.AddIdentityCore<Usuario>();
var identityBuilder = new IdentityBuilder(objectBuilder.UserType, objectBuilder.Services);
identityBuilder.AddEntityFrameworkStores<SeguridadContexto>();
identityBuilder.AddSignInManager<SignInManager<Usuario>>();
builder.Services.TryAddSingleton(TimeProvider.System);

// Dependencia de MeditR (CQRS)
builder.Services.AddMediatR(typeof(Register.UsuarioRegisterCommand).Assembly);
// Agregar automapper
builder.Services.AddAutoMapper(typeof(Register.UsuarioRegisterHadler));
// Agregar dependencias
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IUsuarioSesion, UsuarioSesion>();

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

// Agregar validaciones con Fluent
builder.Services.AddControllers().AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Register>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsRule", rule =>
    {
        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
    });
});

var app = builder.Build();
app.UseCors("CorsRule");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
