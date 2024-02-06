using Newtonsoft.Json;

namespace AC.Library.Core.Models.Protocol
{
    internal class DeviceInfoResponsePack
    {
        [JsonProperty("bc")]
        public string BrandCode { get; set; } = string.Empty;

        [JsonProperty("brand")]
        public string Brand { get; set; } = string.Empty;

        [JsonProperty("catalog")]
        public string Catalog { get; set; } = string.Empty;

        [JsonProperty("mac")]
        public string ClientId { get; set; } = string.Empty;

        [JsonProperty("mid")]
        public string ModelId { get; set; } = string.Empty;

        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string FriendlyName { get; set; } = string.Empty;

        [JsonProperty("series")]
        public string Series { get; set; } = string.Empty;

        [JsonProperty("vender")]
        public string Vendor { get; set; } = string.Empty;

        [JsonProperty("ver")]
        public string FirmwareVersion { get; set; } = string.Empty;

        [JsonProperty("lock")]
        public int LockState { get; set; }
    }
}