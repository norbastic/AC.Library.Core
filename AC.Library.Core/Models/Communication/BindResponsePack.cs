using Newtonsoft.Json;

namespace AC.Library.Core.Models.Communication
{
    public class BindResponsePack
    {
        [JsonProperty("mac")]
        public string MAC { get; set; } = string.Empty;

        [JsonProperty("key")]
        public string Key { get; set; } = string.Empty;
    }
}

