using NSubstitute;
using NUnit.Framework;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1;
using ConsoleApp1.HttpListenerWrapper;
using ConsoleApp1.WebSocketWrapper;
using NSubstitute.ReceivedExtensions;

namespace TestProject1;

[TestFixture]
public class WebSocketServerTests
{
    private IHttpListener _httpListener;
    private IHttpListenerContext _context;
    private IHttpListenerPrefixCollection _prefixCollection;
    private IHttpListenerRequest _request;
    private IHttpListenerResponse _response;
    private IWebSocketContext _webSocketContext;
    private WebSocket _webSocket;

    [SetUp]
    public void SetUp()
    {
        _httpListener = Substitute.For<IHttpListener>();
        _context = Substitute.For<IHttpListenerContext>();
        _prefixCollection = Substitute.For<IHttpListenerPrefixCollection>();
        _request = Substitute.For<IHttpListenerRequest>();
        _response = Substitute.For<IHttpListenerResponse>();
        _webSocketContext = Substitute.For<IWebSocketContext>();
        _webSocket = Substitute.For<WebSocket>();
        
        _context.Request.Returns(_request);
        _context.Response.Returns(_response);

        _httpListener.Prefixes.Returns(_prefixCollection);
        _httpListener.GetContextAsync().Returns(_context);
    }

    [Test]
    public async Task StartAsync_WithValidWebSocketRequest_ShouldInitializeGameAndHandleConnection()
    {
        _request.IsWebSocketRequest.Returns(true);
        _request.Url.Returns(new Uri("http://localhost:8080/game/123"));
        _context.AcceptWebSocketAsync(null).Returns(Task.FromResult(_webSocketContext));

        _webSocketContext.WebSocket.Returns(_webSocket);
        
        _webSocket.ReceiveAsync(Arg.Any<ArraySegment<byte>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new WebSocketReceiveResult(1, WebSocketMessageType.Text, true)));

        _webSocket.State.Returns(WebSocketState.Open, WebSocketState.Closed); // Change state after first receive
        
        var server = new WebSocketServer(_httpListener);

        var serverTask = Task.Run(() => server.StartAsync());
        await Task.Delay(100);

        _httpListener.Received().Start();
        _prefixCollection.Received().Add("http://localhost:8080/");
        _httpListener.DidNotReceive().Close();

        await _context.Received().AcceptWebSocketAsync(null);
        
        await _webSocket.Received().ReceiveAsync(Arg.Any<ArraySegment<byte>>(), Arg.Any<CancellationToken>());

        Assert.Pass("Server started with valid WebSocket request.");

        await serverTask;
    }

    [Test]
    public async Task StartAsync_WithInvalidUri_ShouldNotHandleConnection()
    {
        _request.IsWebSocketRequest.Returns(true);
        _request.Url.Returns(new Uri("http://localhost:8080/wrong-uri"));
        
        var server = new WebSocketServer(_httpListener);

        var serverTask = Task.Run(() => server.StartAsync());
        await Task.Delay(100);

        _httpListener.Received().Start();
        _context.Response.Received().StatusCode = 400;
        _context.Response.Received().Close();
        _httpListener.Received().Close();

        Assert.Pass("Server started and stopped with wrong URI.");

        await serverTask;
    }
    
    [Test]
    public async Task StartAsync_WithNoWebSocketRequest_ShouldNotHandleConnection()
    {
        _request.IsWebSocketRequest.Returns(false);
        _request.Url.Returns(new Uri("http://localhost:8080/game/123"));
        
        var server = new WebSocketServer(_httpListener);

        var serverTask = Task.Run(() => server.StartAsync());
        await Task.Delay(100);

        _httpListener.Received().Start();
        _context.Response.Received().StatusCode = 400;
        _context.Response.Received().Close();
        _httpListener.Received().Close();

        Assert.Pass("Server started and stopped with wrong request.");

        await serverTask;
    }
    
    [TearDown]
    public void TearDown()
    {
        _httpListener.Close();
        _webSocket.Dispose();
    }
}