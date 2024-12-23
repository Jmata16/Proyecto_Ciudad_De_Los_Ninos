﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Proyecto_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Models
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Jovenes> Jovenes { get; set; }
        public DbSet<Expedientes> Expedientes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reportes_Expedientes> Reportes_Expedientes { get; set; }
        public DbSet<Reportes_Medicos> Reportes_Medicos { get; set; }
        public DbSet<Pruebas_Dopaje> Pruebas_Dopaje { get; set; }
        public DbSet<Incidentes> Incidentes { get; set; }
        public DbSet<Citas> Citas { get; set; }
        public DbSet<Inventario_Comedor> Inventario_Comedor { get; set; }
        public DbSet<Inventario_Higiene_Personal> Inventario_Higiene_Personal { get; set; }
        public DbSet<RegistroCompra> RegistroCompra { get; set; }
        public DbSet<Tickete> Tickete { get; set; }
        public DbSet<Rifa> Rifas { get; set; }
        public DbSet<RifaEntry> RifaEntries { get; set; }

        public DbSet<Asistencia> Asistencia { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Jovenes>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("Id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.cedula)
                      .HasColumnName("cedula")
                      .IsRequired();

                entity.Property(e => e.nombre)
                      .HasColumnName("nombre")
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.edad)
                      .HasColumnName("edad")
                      .IsRequired();

                entity.Property(e => e.direccion)
                      .HasColumnName("direccion")
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Localizacion)
                      .HasColumnName("Localizacion")
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.telefono_contacto)
                      .HasColumnName("telefono_contacto")
                      .IsRequired()
                      .HasMaxLength(15)
                      .IsFixedLength();

                // Configuración de relaciones
                entity.HasMany(e => e.Expedientes)
                      .WithOne(e => e.Joven)
                      .HasForeignKey(e => e.id_joven);

                entity.HasMany(e => e.ReportesMedicos)
                      .WithOne(e => e.Joven)
                      .HasForeignKey(e => e.id_joven);

                entity.HasMany(e => e.Pruebas_Dopaje)
                      .WithOne(e => e.Joven)
                      .HasForeignKey(e => e.id_joven);

                entity.HasMany(e => e.Incidentes)
                      .WithOne(e => e.Joven)
                      .HasForeignKey(e => e.id_joven);

                entity.HasMany(e => e.Citas)
                      .WithOne(e => e.Joven)
                      .HasForeignKey(e => e.id_joven);
            });

            // Configuración adicional de los modelos para Reportes_Medicos
            modelBuilder.Entity< Reportes_Medicos> (entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.id_usuario).HasColumnName("id_usuario");
                entity.Property(e => e.id_joven).HasColumnName("id_joven");
                entity.Property(e => e.fecha_creacion).HasColumnName("fecha_creacion");
                entity.Property(e => e.contenido).HasColumnName("contenido");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.ReportesMedicos)
                    .HasForeignKey(d => d.id_usuario);

                entity.HasOne(d => d.Joven)
                    .WithMany(p => p.ReportesMedicos)
                    .HasForeignKey(d => d.id_joven);
            });
            // Configuración adicional de los modelos para Expedientes
            modelBuilder.Entity<Expedientes>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.id_joven).HasColumnName("id_joven");
                entity.Property(e => e.nombre_joven).HasColumnName("nombre_joven");
                entity.Property(e => e.fecha_ingreso).HasColumnName("fecha_ingreso");
                entity.Property(e => e.tutor_legal).HasColumnName("tutor_legal");
                entity.Property(e => e.antecedentes_medicos).HasColumnName("antecedentes_medicos");
                entity.Property(e => e.historial_academico).HasColumnName("historial_academico");
                entity.Property(e => e.notas_adicionales).HasColumnName("notas_adicionales");

                entity.HasOne(d => d.Joven)
                    .WithMany(p => p.Expedientes)
                    .HasForeignKey(d => d.id_joven);
            });
            // Configuración adicional de los modelos para Citas
            modelBuilder.Entity<Citas>(entity =>
            {
                entity.Property(e => e.id_usuario).HasColumnName("id_usuario");
                entity.Property(e => e.id_joven).HasColumnName("id_joven");
                entity.Property(e => e.fecha).HasColumnName("fecha");
                entity.Property(e => e.detalles).HasColumnName("detalles");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Citas)
                    .HasForeignKey(d => d.id_usuario);

                entity.HasOne(d => d.Joven)
                    .WithMany(p => p.Citas)
                    .HasForeignKey(d => d.id_joven);
            });

            // Configuración adicional de los modelos para Pruebas_Dopaje
            modelBuilder.Entity<Pruebas_Dopaje>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("Id")
                      .ValueGeneratedOnAdd();
                entity.Property(e => e.id_usuario).HasColumnName("id_usuario");
                entity.Property(e => e.id_joven).HasColumnName("id_joven");
                entity.Property(e => e.fecha).HasColumnName("fecha");
                entity.Property(e => e.lugar).HasColumnName("lugar");
                entity.Property(e => e.resultado).HasColumnName("resultado");
                entity.Property(e => e.observaciones).HasColumnName("observaciones");

                entity.HasOne(d => d.Usuario)
                      .WithMany(p => p.Pruebas_Dopaje)
                      .HasForeignKey(d => d.id_usuario)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Joven)
                      .WithMany(p => p.Pruebas_Dopaje)
                      .HasForeignKey(d => d.id_joven)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });
            // Configuración adicional de los modelos para Incidentes
            modelBuilder.Entity<Incidentes>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.id_usuario).HasColumnName("id_usuario");
                entity.Property(e => e.id_joven).HasColumnName("id_joven");
                entity.Property(e => e.fecha_hora).HasColumnName("fecha_hora");
                entity.Property(e => e.descripcion).HasColumnName("descripcion");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Incidentes)
                    .HasForeignKey(d => d.id_usuario);

                entity.HasOne(d => d.Joven)
                    .WithMany(p => p.Incidentes)
                    .HasForeignKey(d => d.id_joven);
            });
            // Configuración para Reportes_Expedientes
            modelBuilder.Entity<Reportes_Expedientes>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.id_expediente).HasColumnName("id_expediente");
                entity.Property(e => e.id_usuario).HasColumnName("id_usuario");
                entity.Property(e => e.tipo).HasColumnName("tipo");
                entity.Property(e => e.contenido).HasColumnName("contenido");
                entity.Property(e => e.fecha_creacion).HasColumnName("fecha_creacion");

                entity.HasOne(d => d.Expedientes)
                    .WithMany(p => p.Reportes_Expedientes)
                    .HasForeignKey(d => d.id_expediente);

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Reportes_Expedientes)
                    .HasForeignKey(d => d.id_usuario);
            });
            modelBuilder.Entity<RegistroCompra>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.TicketeId).HasColumnName("TicketeId");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.estado).HasColumnName("estado");
                entity.Property(e => e.Inventario_HigieneId).HasColumnName("Inventario_HigieneId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RegistroCompras)
                    .HasForeignKey(d => d.UserId);

                entity.HasOne(d => d.Inventario_Higiene)
                    .WithMany()
                    .HasForeignKey(d => d.Inventario_HigieneId);
            });


            // Configuración adicional de los modelos para Roles y User
            modelBuilder.Entity<Roles>().HasIndex(r => r.nombre_rol).IsUnique();

            modelBuilder.Entity<User>()
                 .HasIndex(u => u.nombre_usuario)
                 .IsUnique();

        }
        public DbSet<Proyecto_Ciudad_De_Los_Ninos.Models.Capacitaciones> Capacitaciones { get; set; } = default!;
        public DbSet<Proyecto_Ciudad_De_Los_Ninos.Models.Vacaciones> Vacaciones { get; set; } = default!;

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
