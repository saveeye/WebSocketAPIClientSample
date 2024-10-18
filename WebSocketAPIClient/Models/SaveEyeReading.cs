namespace WebSocketAPIClient.Models
{    
    public class SaveEyeReading
    {
        public Guid device_id { get; set; }
        public string? saveeyeDeviceSerialNumber { get; set; }
        public string? meterType { get; set; }
        public string? meterSerialNumber { get; set; }
        public string timestamp { get; set; } = default!;
        public double wifiRssi { get; set; }
        public ActiveActualConsumption? activeActualConsumption { get; set; }
        public ActiveActualProduction? activeActualProduction { get; set; }
        public ActiveTotalConsumption? activeTotalConsumption { get; set; }
        public ActiveTotalProduction? activeTotalProduction { get; set; }
        public ReactiveActualConsumption? reactiveActualConsumption { get; set; }
        public ReactiveActualProduction? reactiveActualProduction { get; set; }
        public ReactiveTotalConsumption? reactiveTotalConsumption { get; set; }
        public ReactiveTotalProduction? reactiveTotalProduction { get; set; }
        public RmsVoltage? rmsVoltage { get; set; }
        public RmsCurrent? rmsCurrent { get; set; }
        public PowerFactor? powerFactor { get; set; }
    }
}
