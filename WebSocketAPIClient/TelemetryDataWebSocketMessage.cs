using System.Text.Json.Serialization;

namespace WebSocketAPIClient
{
    public class TelemetryDataWebSocketMessage
    {
        [JsonPropertyName("deviceId")]
        public required Guid DeviceId { get; set; }

        [JsonPropertyName("saveEyeDeviceSerialNumber")]
        public required string SaveEyeDeviceSerialNumber { get; set; }

        [JsonPropertyName("timestamp")]
        public required DateTime TimestampUtc { get; set; }

        [JsonPropertyName("currentConsumptionW")]
        public EnergyMeasurementW? CurrentConsumptionW { get; set; }

        [JsonPropertyName("currentProductionW")]
        public EnergyMeasurementW? CurrentProductionW { get; set; }

        [JsonPropertyName("totalConsumptionWh")]
        public EnergyMeasurementWh? TotalConsumptionWh { get; set; }

        [JsonPropertyName("totalProductionWh")]
        public EnergyMeasurementWh? TotalProductionWh { get; set; }

        [JsonPropertyName("reactiveCurrentConsumptionW")]
        public EnergyMeasurementW? ReactiveCurrentConsumptionW { get; set; }

        [JsonPropertyName("reactiveCurrentProductionW")]
        public EnergyMeasurementW? ReactiveCurrentProductionW { get; set; }

        [JsonPropertyName("reactiveTotalConsumptionWh")]
        public EnergyMeasurementWh? ReactiveTotalConsumptionWh { get; set; }

        [JsonPropertyName("reactiveTotalProductionWh")]
        public EnergyMeasurementWh? ReactiveTotalProductionWh { get; set; }

        [JsonPropertyName("rmsVoltage")]
        public PhaseMeasurements? RmsVoltage { get; set; }

        [JsonPropertyName("rmsCurrent")]
        public PhaseMeasurements? RmsCurrent { get; set; }

        [JsonPropertyName("powerFactor")]
        public PowerFactor? PowerFactor { get; set; }
    }

    public class EnergyMeasurementWh
    {
        [JsonPropertyName("total")]
        public required double Total { get; set; }

        [JsonPropertyName("l1")]
        public double? L1 { get; set; }

        [JsonPropertyName("l2")]
        public double? L2 { get; set; }

        [JsonPropertyName("l3")]
        public double? L3 { get; set; }
    }

    public class EnergyMeasurementW
    {
        [JsonPropertyName("total")]
        public required double Total { get; set; }

        [JsonPropertyName("l1")]
        public double? L1 { get; set; }

        [JsonPropertyName("l2")]
        public double? L2 { get; set; }

        [JsonPropertyName("l3")]
        public double? L3 { get; set; }
    }

    public class PhaseMeasurements
    {
        [JsonPropertyName("l1")]
        public double? L1 { get; set; }

        [JsonPropertyName("l2")]
        public double? L2 { get; set; }

        [JsonPropertyName("l3")]
        public double? L3 { get; set; }
    }

    public class PowerFactor
    {
        [JsonPropertyName("total")]
        public required double Total { get; set; }

        [JsonPropertyName("l1")]
        public double? L1 { get; set; }

        [JsonPropertyName("l2")]
        public double? L2 { get; set; }

        [JsonPropertyName("l3")]
        public double? L3 { get; set; }
    }
}