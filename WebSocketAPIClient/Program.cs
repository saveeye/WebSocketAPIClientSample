using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebSocketAPIClient;

Console.WriteLine("SaveEye WebSocket API Client\n");

// TODO: Add your own API-KEY and DeviceId
var apiKey = "[INSERT_YOUR_API_KEY_HERE]";
var deviceId = "[INSERT_A_DEVICE_ID_HERE]";

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

        client.Options.SetRequestHeader("x-api-key", apiKey);

        Console.WriteLine("Attempting to connect to WebSocket...");
        await client.ConnectAsync(new Uri($"wss://realtime.prod.saveeye.dk?deviceId={deviceId}"), cts.Token);
        Console.WriteLine("Connection established.\n\n");

        // Start by listening
        await ReceiveMessagesAsync(client, cts.Token);
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
                    var data = JsonSerializer.Deserialize<TelemetryDataWebSocketMessage>(message);

                    if (data != null)
                    {
                        // Do whatever you want
                        Console.WriteLine($"Timestamp: {data?.TimestampUtc}");
                        Console.WriteLine($"CurrentConsumption: {data?.CurrentConsumptionW?.Total} Wh");
                        Console.WriteLine($"CurrentProduction: {data?.CurrentProductionW?.Total} Wh");
                        Console.WriteLine($"TotalConsumption: {data?.TotalConsumptionWh?.Total} Wh");
                        Console.WriteLine($"TotalProduction: {data?.TotalProductionWh?.Total} Wh");
                        Console.WriteLine($"---------------------------------------------------");
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
