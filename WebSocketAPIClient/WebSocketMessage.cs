using System.Text.Json.Serialization;

namespace WebSocketAPIClient
{
    public class WebSocketMessage
    {
        [JsonPropertyName("action")]
        public string? Action { get; set; }

        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }

    }
}
