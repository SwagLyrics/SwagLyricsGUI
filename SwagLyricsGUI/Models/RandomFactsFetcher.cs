using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SwagLyricsGUI.Models
{
    public static class RandomFactsFetcher
    {
        public const string FactsUrl = @"https://uselessfacts.jsph.pl/random.json?language=en";
        public static Fact GetRandomFact()
        {
            WebRequest request = WebRequest.Create(FactsUrl);
            var response = request.GetResponse();
            Fact result;

            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                result = JsonSerializer.Deserialize<Fact>(reader.ReadToEnd());
            }

            // Close the response.
            response.Close();
            return result;
        }
    }
}
