using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Models
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Jovenes> Jovenes { get; set; }
        public DbSet<Expedientes> Expedientes { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Reportes_Expedientes> ReportesExpedientes { get; set; }
        public DbSet<Reportes_Medicos> ReportesMedicos { get; set; }
        public DbSet<Pruebas_Dopaje> PruebasDopaje { get; set; }
        public DbSet<Incidentes> Incidentes { get; set; }
        public DbSet<Citas> Citas { get; set; }
        public DbSet<Inventario_Comedor> Inventario_Comedor { get; set; }
        public DbSet<Inventario_Higiene_Personal> Inventario_Higiene_Personal { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración adicional de los modelos
            modelBuilder.Entity<Roles>().HasIndex(r => r.nombre_rol)
                       .IsUnique();

            modelBuilder.Entity<User>()
                       .HasIndex(u => u.nombre_usuario).IsUnique();
        }



        //No tocar parte Roles
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.UserId, x.RoleId });
        //    modelBuilder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
        //    modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(x => x.Id);
        //    modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey });
        //    modelBuilder.Entity<IdentityRoleClaim<string>>().HasKey(x => x.Id);
        //}

    }
}

