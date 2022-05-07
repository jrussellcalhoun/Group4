using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WordGame.Utilities
{
    public class WordDefinition
    {
        [JsonPropertyName("definition")] public string Definition { get; set; }
        [JsonPropertyName("example")] public string Example { get; set; }
    }
}
