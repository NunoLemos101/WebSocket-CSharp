namespace ConsoleApp1.HttpListenerWrapper;

public interface IHttpListenerResponse
{
    int StatusCode { get; set; }
    void Close();
}