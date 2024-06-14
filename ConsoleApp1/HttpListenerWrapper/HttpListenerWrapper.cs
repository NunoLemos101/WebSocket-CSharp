using System.Collections;
using System.Net;
using System.Net.WebSockets;
using ConsoleApp1.HttpListenerWrapper;

namespace ConsoleApp1.HttpListenerWrapper;

public class HttpListenerWrapper : IHttpListener
{
    private readonly HttpListener _httpListener;

    public HttpListenerWrapper(HttpListener httpListener)
    {
        _httpListener = httpListener;
        Prefixes = new HttpListenerPrefixCollectionWrapper(_httpListener.Prefixes);
    }

    public Task<IHttpListenerContext> GetContextAsync()
    {
        return _httpListener.GetContextAsync().ContinueWith(
            t => new HttpListenerContextWrapper(t.Result) as IHttpListenerContext);
    }

    public void Start() => _httpListener.Start();

    public void Close() => _httpListener.Close();

    public void Stop() => _httpListener.Stop();
    public IHttpListenerPrefixCollection Prefixes { get; }
}