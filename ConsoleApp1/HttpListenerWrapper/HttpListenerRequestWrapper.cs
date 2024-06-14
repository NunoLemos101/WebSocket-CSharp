using System.Net;

namespace ConsoleApp1.HttpListenerWrapper;

public class HttpListenerRequestWrapper : IHttpListenerRequest
{
    private readonly HttpListenerRequest _request;

    public HttpListenerRequestWrapper(HttpListenerRequest request)
    {
        _request = request;
    }

    public bool IsWebSocketRequest => _request.IsWebSocketRequest;

    public Uri Url => _request.Url;
}