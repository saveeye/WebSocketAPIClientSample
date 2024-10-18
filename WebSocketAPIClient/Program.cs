using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebSocketAPIClient;
using WebSocketAPIClient.Models;

Console.WriteLine("SaveEye WebSocket API Client\n");

using (var cts = new CancellationTokenSource())
using (var client = new ClientWebSocket())
{
    try
    {
        // Handle Ctrl+C for graceful shutdown
        Console.CancelKeyPress += (sender, e) => {
            Console.WriteLine("Closing connection due to user request...");
            cts.Cancel();
        };

        // TODO: Add your own API-KEY
        client.Options.SetRequestHeader("x-api-key", "[INSERT_YOUR_API_KEY_HERE]");

        Console.WriteLine("Attempting to connect to WebSocket...");
        await client.ConnectAsync(new Uri("wss://realtime.prod.saveeye.dk/"), cts.Token);
        Console.WriteLine("Connection established.");

        // Start by listening - to catch Acknowledgement-messages
        var receiveMessagesTask = ReceiveMessagesAsync(client, cts.Token);

        // Subscribe to specific device
        // TODO: Add a deviceId of one of your devices (get from either GraphQL or REST API)
        await SendMessageAsync(client, new WebSocketMessage
        {
            Action = "subscribe",
            DeviceId = "[INSERT_A_DEVICE_ID_HERE]" // Like "0a6c0540-83fd-11ee-b4fb-d35174fa41ae"
        }, cts.Token);

        // Example of subscribing to multiple devices
        // await SendMessageAsync(client, new WebSocketMessage
        // {
        //     Action = "subscribe",
        //     DeviceId = "[ANOTHER_DEVICE_ID_HERE]"
        // }, cts.Token);

        // Start receiving messages
        await receiveMessagesTask;
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
            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", cts.Token);
        }

        Console.WriteLine("Connection closed.");
    }
}

async Task SendMessageAsync(ClientWebSocket client, WebSocketMessage message, CancellationToken cancellationToken)
{
    var messageJson = JsonSerializer.Serialize(message);

    byte[] buffer = Encoding.UTF8.GetBytes(messageJson);
    
    await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
    
    Console.WriteLine($"Sent: {messageJson}");
}

async Task ReceiveMessagesAsync(ClientWebSocket client, CancellationToken cancellationToken)
{
    var buffer = new byte[1024 * 16];
    while (client.State == WebSocketState.Open)
    {
        try
        {
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server closed", cancellationToken);
                Console.WriteLine("Server closed the connection.");
            }
            else
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                try
                {
                    var receivedMessage = JsonSerializer.Deserialize<WebSocketMessage>(message);

                    if (receivedMessage.Action == "ackSubscription")
                    {
                        Console.WriteLine($"Received ACK: Subscribed to device {receivedMessage.DeviceId}");
                    }
                    else if (!string.IsNullOrEmpty(receivedMessage.Action))
                    {
                        Console.WriteLine($"Received ACK: Action {receivedMessage.Action}");
                    }
                    else
                    {
                        var saveEyeReading = JsonSerializer.Deserialize<SaveEyeReading>(message);

                        // Do whatever you want

                        Console.WriteLine($"CurrentConsumption: {saveEyeReading.activeActualConsumption.total}");
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error parsing message: {ex.Message}");
                    Console.WriteLine($"Received raw message: {message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error receiving message: {ex.Message}");
        }        
    }
}
