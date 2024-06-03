using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Models
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Joven> Jovenes { get; set; }
        public DbSet<Expediente> Expedientes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ReporteExpediente> ReportesExpedientes { get; set; }
        public DbSet<ReporteMedico> ReportesMedicos { get; set; }
        public DbSet<PruebaDopaje> PruebasDopaje { get; set; }
        public DbSet<Incidente> Incidentes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<InventarioComedor> InventarioComedor { get; set; }
        public DbSet<InventarioHigienePersonal> InventarioHigienePersonal { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración adicional de los modelos
            modelBuilder.Entity<Rol>().HasIndex(r => r.NombreRol)
                       .IsUnique();

            modelBuilder.Entity<User>()
                       .HasIndex(u => u.NombreUsuario).IsUnique();
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

