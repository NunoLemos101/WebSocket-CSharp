using System.Collections;
using System.Net;
using System.Net.WebSockets;

namespace ConsoleApp1.HttpListenerWrapper;

public interface IHttpListener
{
    Task<IHttpListenerContext> GetContextAsync();
    void Start();
    void Close();
    void Stop();
    IHttpListenerPrefixCollection Prefixes { get; }
}