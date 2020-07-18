using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SwagLyricsGUI.Models
{
    public class Fact
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("source_url")]
        public string SourceUrl { get; set; }

        public Fact(string text, string source)
        {
            Text = text;
            SourceUrl = source;
        }

        public Fact()
        {

        }

        public override string ToString() => $"{Text}\n\n\nSource: {SourceUrl}";
    }
}
