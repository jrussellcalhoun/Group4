using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Word_Game.Utilities
{
    public class WordMeaning
    {
        [JsonPropertyName("partOfSpeech")] public string PartOfSpeech { get; set; }
        [JsonPropertyName("definitions")] public IList<WordDefinition> Type { get; set; }
    }
}
