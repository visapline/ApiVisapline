using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using ApiPruebaVive.Hubs;

namespace ApiPruebaVive.Services
{
    public class OltStatusBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<OltHub> _hubContext;

        public OltStatusBackgroundService(IServiceProvider serviceProvider, IHubContext<OltHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var tarjetas = new[] { "2", "3", "4", "5", "6", "7", "8", "9", "12", "13", "14", "15", "16", "17" };

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var oltStatusService = scope.ServiceProvider.GetRequiredService<IOltStatusService>();

                var tasks = tarjetas.Select(async tarjeta =>
                {
                    try
                    {
                        var puertos = await oltStatusService.ConsultarPuertosDeTarjeta(tarjeta);
                        var timestamp = DateTime.Now;

                        Console.WriteLine($"✅ [{timestamp:HH:mm:ss}] Tarjeta {tarjeta} actualizada. Enviando a clientes...");

                        await _hubContext.Clients.All.SendAsync("RecibirEstadoPorTarjeta", new
                        {
                            tarjeta,
                            puertos,
                            timestamp = timestamp.ToString("o") // formato ISO 8601 (ej: "2025-05-21T12:34:56.789Z")
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ [{DateTime.Now:HH:mm:ss}] Error al consultar tarjeta {tarjeta}: {ex.Message}");
                    }
                });

                await Task.WhenAll(tasks);

                await Task.Delay(5000, stoppingToken); // Pausa entre ciclos de actualización
            }
        }
    }
}
