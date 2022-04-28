using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

// Tutorial on this code: https://ramihikmat.net/Article/2021-implementing-a-dictionary-api-using-real-world-best-practices-using-c%23

namespace Word_Game.Utilities
{
    public class DictionaryWord
    {
        [JsonPropertyName("word")] public string WordName { get; set; }

        [JsonPropertyName("meanings")] public IList<WordMeaning> Meanings { get; set; }
    }
}
