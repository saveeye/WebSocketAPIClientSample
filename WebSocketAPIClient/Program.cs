using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebSocketAPIClient;

Console.WriteLine("SaveEye WebSocket API Client\n");

using (var cts = new CancellationTokenSource())
using (var client = new ClientWebSocket())
{
    try
    {
        // TODO: Add your own API-KEY
        client.Options.SetRequestHeader("x-api-key", "[INSERT_YOUR_API_KEY_HERE]");

        await client.ConnectAsync(new Uri("wss://realtime.prod.saveeye.dk/"), CancellationToken.None);

        // Subscribe to specific device
        // TODO: Add a deviceId of one of your devices (get from either GraphQL or REST API)
        await SendMessageAsync(client, new WebSocketMessage
        {
            Action = "subscribe",
            DeviceId = "[INSERT_A_DEVICE_ID_HERE]" // Like "0a6c0540-83fd-11ee-b4fb-d35174fa41ae"
        });

        // Possible to send additional subscribe messages if you want to subscribe to multiple devices in the same connection


        // Start receiving messages
        Console.WriteLine("\nWaiting to receive messages....\n");
        var buffer = new byte[1024 * 4];
        while (client.State == WebSocketState.Open)
        {
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

            Console.WriteLine($"Received message: {message}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception thrown: {ex}");
    }
    finally
    {
        // Making sure to close connection
        if (client.State == WebSocketState.Open)
        {
            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", CancellationToken.None);
        }

        Console.WriteLine("Connection closed.");
    }
}

async Task SendMessageAsync(ClientWebSocket client, WebSocketMessage message)
{
    var messageJson = JsonSerializer.Serialize(message);

    byte[] buffer = Encoding.UTF8.GetBytes(messageJson);
    
    await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    
    Console.WriteLine($"Sent: {messageJson}");
}
