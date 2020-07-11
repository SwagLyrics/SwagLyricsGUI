using System;

namespace SwagLyricsGUI.Models
{
    public class NewSongEventArgs : EventArgs
    {
        public string Song { get; set; }

        public NewSongEventArgs(string song)
        {
            Song = song;
        }
    }
}
