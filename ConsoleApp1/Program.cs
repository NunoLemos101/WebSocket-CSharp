using System.Net;

namespace ConsoleApp1
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var httpListener = new HttpListener();
            var httpListenerWrapper = new HttpListenerWrapper.HttpListenerWrapper(httpListener);
            var server = new WebSocketServer(httpListenerWrapper);

            await server.StartAsync();
        }
    }
}