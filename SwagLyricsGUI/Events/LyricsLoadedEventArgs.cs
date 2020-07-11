using System;

namespace SwagLyricsGUI.Models
{
    public class LyricsLoadedEventArgs : EventArgs
    {
        public string Lyrics { get; set; }

        public LyricsLoadedEventArgs(string lyrics)
        {
            Lyrics = lyrics;
        }
    }
}
