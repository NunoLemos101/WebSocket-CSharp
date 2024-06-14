using System.Net.WebSockets;
using System.Security.Principal;

namespace ConsoleApp1.WebSocketWrapper;

public interface IWebSocketContext
{
    WebSocket WebSocket { get; }
    IPrincipal User { get; }
    bool IsAuthenticated { get; }
    bool IsLocal { get; }
    bool IsSecureConnection { get; }
    Uri RequestUri { get; }
    string Origin { get; }
}