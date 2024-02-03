using System.Collections.Generic;
using Newtonsoft.Json;

namespace AC.Library.Core.Models.Communication
{
    internal class CommandResponsePack
    {
        [JsonProperty("opt")]
        public List<string> Columns { get; set; } = new List<string>();

        [JsonProperty("p")]
        public List<int> Values { get; set; } = new List<int>();
    }
}


