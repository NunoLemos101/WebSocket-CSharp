namespace ConsoleApp1.HttpListenerWrapper;

public interface IHttpListenerRequest
{
    bool IsWebSocketRequest { get; }
    Uri Url { get; }
}