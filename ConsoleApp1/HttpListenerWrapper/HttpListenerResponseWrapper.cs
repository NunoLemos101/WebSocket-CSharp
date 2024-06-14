using System.Net;

namespace ConsoleApp1.HttpListenerWrapper;

public class HttpListenerResponseWrapper : IHttpListenerResponse
{
    private readonly HttpListenerResponse _response;

    public HttpListenerResponseWrapper(HttpListenerResponse response)
    {
        _response = response;
    }

    public int StatusCode
    {
        get => _response.StatusCode;
        set => _response.StatusCode = value;
    }

    public void Close() => _response.Close();
}