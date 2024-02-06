using System.Collections.Generic;
using Newtonsoft.Json;

namespace AC.Library.Core.Models.Protocol
{
    public class StatusRequestPack
    {
        [JsonProperty("t")]
        public string Type { get; set; }
        [JsonProperty("mac")]
        public string MAC { get; set; }
        [JsonProperty("cols")]
        public List<string> Columns { get; set; }
    }
}

