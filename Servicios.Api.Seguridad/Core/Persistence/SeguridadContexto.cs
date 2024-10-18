using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Servicios.Api.Seguridad.Core.Entities;

namespace Servicios.Api.Seguridad.Core.Persistence
{
    public class SeguridadContexto : IdentityDbContext<Usuario>
    {
        public SeguridadContexto(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
