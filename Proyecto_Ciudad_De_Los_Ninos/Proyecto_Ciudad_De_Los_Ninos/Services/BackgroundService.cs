using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Proyecto_Ciudad_De_Los_Ninos.Models;

public class RifaBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public RifaBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await DetermineGanadoresAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Verifica cada minuto
        }
    }

    private async Task DetermineGanadoresAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
            var now = DateTime.Now;

            var rifas = context.Rifas
                .Where(r => r.FechaRifa <= now && r.NumeroGanador == null)
                .ToList();

            foreach (var rifa in rifas)
            {
                var random = new Random();
                rifa.NumeroGanador = random.Next(1, 101);
                context.Rifas.Update(rifa);
            }

            await context.SaveChangesAsync(stoppingToken);
        }
    }
}
