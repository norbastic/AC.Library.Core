using Newtonsoft.Json;

namespace AC.Library.Core.Models.Communication
{
    internal class PackInfo
    {
        [JsonProperty("t")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("cid")]
        public string ClientId { get; set; } = string.Empty;
    }

    internal class ResponsePackInfo : PackInfo
    {
        [JsonProperty("uid")]
        [JsonIgnore]
        public int? UID { get; set; }

        [JsonProperty("tcid")]
        public string TargetClientId { get; set; } = string.Empty;

        [JsonProperty("pack")]
        public string Pack { get; set; } = string.Empty;
    }
}

