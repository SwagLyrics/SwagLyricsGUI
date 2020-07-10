using ReactiveUI;
using SwagLyricsGUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwagLyricsGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static MainWindowViewModel Current { get; set; }
        private string _lyrics = "";
        public string Lyrics
        {
            get => _lyrics;
            set => this.RaiseAndSetIfChanged(ref _lyrics, value);
        }
        private SwagLyricsBridge _bridge = new SwagLyricsBridge();

        public MainWindowViewModel()
        {
            Current = this;
            _bridge.GetLyrics();
        }
    }
}
