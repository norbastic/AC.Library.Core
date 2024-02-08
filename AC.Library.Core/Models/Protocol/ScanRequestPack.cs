using Newtonsoft.Json;

namespace AC.Library.Core.Models.Protocol
{
    internal class ScanRequestPack
    {
        [JsonProperty("t")]
        public string Type { get => "scan"; private set { } }
    }
}