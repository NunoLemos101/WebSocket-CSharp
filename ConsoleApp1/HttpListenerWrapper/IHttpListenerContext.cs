using System.Net.WebSockets;
using ConsoleApp1.WebSocketWrapper;

namespace ConsoleApp1.HttpListenerWrapper;

public interface IHttpListenerContext
{
    IHttpListenerRequest Request { get; }
    IHttpListenerResponse Response { get; }
    Task<IWebSocketContext> AcceptWebSocketAsync(string subProtocol);
}