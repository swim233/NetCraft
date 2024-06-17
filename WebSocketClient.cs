// WebSocketClient.cs
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class WebSocketClient
{
    private ClientWebSocket _clientWebSocket = new ClientWebSocket();

    public event EventHandler<string>? MessageReceived;

    public async Task StartAsync()
    {
        await _clientWebSocket.ConnectAsync(
            new Uri("ws://localhost:8080/"),
            CancellationToken.None
        );
        Console.WriteLine("Connected to WebSocket server at ws://localhost:8080/");

        _ = Task.Run(() => ReceiveMessagesAsync());

        while (_clientWebSocket.State == WebSocketState.Open)
        {
            string message = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _clientWebSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }
    }

    private async Task ReceiveMessagesAsync()
    {
        byte[] buffer = new byte[1024];

        while (_clientWebSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None
            );
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            MessageReceived?.Invoke(this, message);
        }
    }
}
