using Newtonsoft.Json;

namespace AC.Library.Core.Models.Protocol
{
    public class StatusResponsePack
    {
        [JsonProperty("t")]
        public string Type { get; set; }
        [JsonProperty("mac")]
        public string MAC { get; set; }
        [JsonProperty("r")]
        public int Rvalue { get; set; }
        [JsonProperty("cols")]
        public string[] Columns { get; set; }
        [JsonProperty("dat")]
        public int[] Data { get; set; }
    }
}