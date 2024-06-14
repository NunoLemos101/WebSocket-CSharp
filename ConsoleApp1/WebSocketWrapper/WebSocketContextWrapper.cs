using System.Net.WebSockets;
using System.Security.Principal;

namespace ConsoleApp1.WebSocketWrapper;

public class WebSocketContextWrapper : IWebSocketContext
{
    private readonly WebSocketContext _context;

    public WebSocketContextWrapper(WebSocketContext context)
    {
        _context = context;
    }

    public WebSocket WebSocket => _context.WebSocket;
    public IPrincipal User => _context.User;
    public bool IsAuthenticated => _context.IsAuthenticated;
    public bool IsLocal => _context.IsLocal;
    public bool IsSecureConnection => _context.IsSecureConnection;
    public Uri RequestUri => _context.RequestUri;
    public string Origin => _context.Origin;
}