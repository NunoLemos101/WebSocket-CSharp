using System.Net.WebSockets;

namespace ConsoleApp1.WebSocketWrapper;

public class WebSocketWrapper : IWebSocket
{
    private readonly WebSocket _webSocket;

    public WebSocketWrapper(WebSocket webSocket)
    {
        _webSocket = webSocket;
    }

    public WebSocketState State => _webSocket.State;

    public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        return _webSocket.ReceiveAsync(buffer, cancellationToken);
    }

    public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
    {
        return _webSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
    }

    public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        return _webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
    }

    public void Dispose()
    {
        _webSocket.Dispose();
    }
}