using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using Microsoft.EntityFrameworkCore;

public class RifaService : BackgroundService
{
    private readonly ILogger<RifaService> _logger;
    private readonly ApplicationDBContext _context;

    public RifaService(ILogger<RifaService> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("RifaService is running.");

            await ProcessRifasAsync();

            // Espera 1 minuto antes de la próxima ejecución
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ProcessRifasAsync()
    {
        var rifas = await _context.Rifas
            .Where(r => r.FechaRifa <= DateTime.Now && r.NumeroGanador == null)
            .ToListAsync();

        foreach (var rifa in rifas)
        {
            var random = new Random();
            rifa.NumeroGanador = random.Next(1, 101);
            await _context.SaveChangesAsync();
        }
    }
}
