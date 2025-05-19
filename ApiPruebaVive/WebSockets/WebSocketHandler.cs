using System.Net.WebSockets;

namespace ApiPruebaVive.WebSockets
{
    public class WebSocketHandler
    {
        private readonly CustomWebSocketManager _manager;

        public WebSocketHandler(CustomWebSocketManager manager)
        {
            _manager = manager;
        }

        public async Task HandleAsync(WebSocket socket)
        {
            await _manager.ReceiveAsync(socket, async (message) =>
            {
                Console.WriteLine($"[WS] Recibido: {message}");

                // Aquí puedes poner lógica personalizada: parsear JSON, llamar servicios, etc.
                await _manager.SendAsync(socket, $"Echo: {message}");
            });
        }
    }
}
