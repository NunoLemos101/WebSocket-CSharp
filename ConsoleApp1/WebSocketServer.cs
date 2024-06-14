using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Collections.Concurrent;
using ConsoleApp1.HttpListenerWrapper;
using ConsoleApp1.WebSocketWrapper;

namespace ConsoleApp1;

public class WebSocketServer
{
    private IHttpListener _httpListener;
    private ConcurrentBag<WebSocket> _clients = new ConcurrentBag<WebSocket>();
    private ConcurrentDictionary<string, ConcurrentBag<IWebSocket>> _gameRooms = new ConcurrentDictionary<string, ConcurrentBag<IWebSocket>>();

    public WebSocketServer(IHttpListener httpListener)
    {
        _httpListener = httpListener;
    }
    
    public async Task StartAsync()
    {
        _httpListener.Prefixes.Add("http://localhost:8080/");
        _httpListener.Start();
        Console.WriteLine("WebSocket server started at ws://localhost:8080/");

        while (true)
        {
            var httpContext = await _httpListener.GetContextAsync();

            var path = httpContext.Request.Url.AbsolutePath;

            var gameId = PathResolver.GetGameIdFromPath(path);
            
            if (httpContext.Request.IsWebSocketRequest && gameId != null)
            {
                Task.Run(() => HandleWebSocketConnectionAsync(httpContext, gameId));
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                httpContext.Response.Close();
                _httpListener.Close();
            }
        }
    }

    private void InitializeGame(string gameId)
    {
        _gameRooms[gameId] = new ConcurrentBag<IWebSocket>();
    }
    
    private async Task HandleWebSocketConnectionAsync(IHttpListenerContext httpContext, string gameId)
    {
        var webSocketContext = await httpContext.AcceptWebSocketAsync(null);
        IWebSocket webSocket = new WebSocketWrapper.WebSocketWrapper(webSocketContext.WebSocket);
        Console.WriteLine("Client connected.");
        
        if (!_gameRooms.ContainsKey(gameId))
        {
            InitializeGame(gameId);
        }
        
        var room = _gameRooms[gameId];
        room.Add(webSocket);

        try
        {
            byte[] buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            
            while (webSocket.State == WebSocketState.Open)
            {
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Game - {gameId}: {receivedMessage}");

                string responseMessage = $"Echo: {receivedMessage}";
                byte[] responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                foreach (var ws in room)
                {
                    await ws.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine($"WebSocket error: {ex.Message}");
        }
        finally
        {
            if (webSocket.State != WebSocketState.Closed)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
            _clients.TryTake(out _);
            Console.WriteLine("Client disconnected.");
            webSocket.Dispose();
        }
    }
}