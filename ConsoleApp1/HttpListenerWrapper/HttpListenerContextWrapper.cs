using System.Net;
using System.Net.WebSockets;
using ConsoleApp1.WebSocketWrapper;

namespace ConsoleApp1.HttpListenerWrapper;

public class HttpListenerContextWrapper : IHttpListenerContext
{
    private readonly HttpListenerContext _context;

    public HttpListenerContextWrapper(HttpListenerContext context)
    {
        _context = context;
    }
    public Task<IWebSocketContext> AcceptWebSocketAsync(string subProtocol)
    {
        return _context.AcceptWebSocketAsync(subProtocol).ContinueWith(
            t => new WebSocketContextWrapper(t.Result) as IWebSocketContext);
    }
    public IHttpListenerRequest Request => new HttpListenerRequestWrapper(_context.Request);

    public IHttpListenerResponse Response => new HttpListenerResponseWrapper(_context.Response);
}