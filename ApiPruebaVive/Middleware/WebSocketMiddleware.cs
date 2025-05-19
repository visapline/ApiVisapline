using ApiPruebaVive.WebSockets;


namespace ApiPruebaVive.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketHandler _handler;

        public WebSocketMiddleware(RequestDelegate next, WebSocketHandler handler)
        {
            _next = next;
            _handler = handler;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using var socket = await context.WebSockets.AcceptWebSocketAsync();
                    await _handler.HandleAsync(socket);
                    return;
                }

                context.Response.StatusCode = 400;
                return;
            }

            await _next(context);
        }
    }
}
