using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

// Tutorial on this code: https://ramihikmat.net/Article/2021-implementing-a-dictionary-api-using-real-world-best-practices-using-c%23

/// <summary>
/// This code was adapted from the tutorial listed above. It is a modern C# way to interface with and utilize dictionary api.
/// We use dictionary api to look up hints for our words (definitions).
/// </summary>
namespace WordGame.Utilities
{
    public class DictionaryWord
    {
        [JsonPropertyName("word")] public string WordName { get; set; }

        [JsonPropertyName("meanings")] public IList<WordMeaning> Meanings { get; set; }
    }
}
