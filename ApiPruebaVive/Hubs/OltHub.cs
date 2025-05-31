using Microsoft.AspNetCore.SignalR;

namespace ApiPruebaVive.Hubs
{
    public class OltHub : Hub
    {
        // Puedes agregar métodos que los clientes pueden invocar aquí si lo necesitas.
        public async Task EnviarMensaje(string mensaje)
        {
            await Clients.All.SendAsync("RecibirMensaje", mensaje);
        }

    }
}
