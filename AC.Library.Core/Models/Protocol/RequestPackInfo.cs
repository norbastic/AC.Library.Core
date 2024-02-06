using Newtonsoft.Json;

namespace AC.Library.Core.Models.Protocol
{
    internal class RequestPackInfo
    {
        [JsonProperty("t")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("uid")]
        [JsonIgnore]
        public int? UID { get; set; }

        [JsonProperty("mac")]
        public string MAC { get; set; } = string.Empty;
    }
}


