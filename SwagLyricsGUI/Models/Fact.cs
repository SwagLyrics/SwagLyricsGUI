using System.Text.Json.Serialization;

namespace SwagLyricsGUI.Models
{
    public class Fact
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        public Fact(string text)
        {
            Text = text;
        }

        public Fact()
        {

        }

        public override string ToString() => Text.ToString();
    }
}
