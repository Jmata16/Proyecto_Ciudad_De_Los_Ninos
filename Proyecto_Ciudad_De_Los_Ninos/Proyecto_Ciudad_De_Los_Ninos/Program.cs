using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configura la cadena de conexión y el DbContext
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configura Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("1"));
    options.AddPolicy("EstudiantePolicy", policy => policy.RequireRole("5"));
    options.AddPolicy("PsicologoPolicy", policy => policy.RequireRole("3"));
    options.AddPolicy("SaludPolicy", policy => policy.RequireRole("2"));
    options.AddPolicy("TrabajadorSocialPolicy", policy => policy.RequireRole("4"));
    options.AddPolicy("VentasPolicy", policy => policy.RequireRole("6"));
    options.AddPolicy("Rol134", policy => policy.RequireRole("1", "3", "4"));
    options.AddPolicy("Rol1234", policy => policy.RequireRole("1", "2", "3", "4"));
    options.AddPolicy("Rol16", policy => policy.RequireRole("1", "6"));
    options.AddPolicy("RolAll", policy => policy.RequireRole("1", "2", "3", "4", "5", "6"));
    options.AddPolicy("Rol14", policy => policy.RequireRole("1", "6"));
    options.AddPolicy("Rol15", policy => policy.RequireRole("1", "5"));
});

// Configura autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.LoginPath = "/Login/Login"; // Ruta de inicio de sesión
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.SlidingExpiration = true;
    });

// Registra el servicio de correo electrónico
builder.Services.AddTransient<EmailService>();

// Agrega el servicio de rifa en segundo plano
builder.Services.AddHostedService<RifaBackgroundService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware de desarrollo y producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStaticFiles();
// Configuraciones generales de middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

// Middleware de enrutamiento
app.UseRouting();

// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Configuración de enrutamiento para controladores y vistas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ejecución de la aplicación
app.Run();

