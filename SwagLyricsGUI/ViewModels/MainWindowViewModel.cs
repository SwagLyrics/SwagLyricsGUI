using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Platform;
using Avalonia.Styling;
using ReactiveUI;
using SwagLyricsGUI.Models;
using SwagLyricsGUI.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SwagLyricsGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public List<IStyle> Themes { get; set; }
        public static MainWindowViewModel Current { get; set; }

        private int _themeIndex = 1;
        public int ThemeIndex
        {
            get => _themeIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _themeIndex, value);
                ChangeTheme(value);
            }
        }

        private string _lyrics = "";
        public string Lyrics
        {
            get => _lyrics;
            set => this.RaiseAndSetIfChanged(ref _lyrics, value);
        }

        private string _song = "Nothing is playing";
        public string Song
        {
            get => _song;
            set => this.RaiseAndSetIfChanged(ref _song, value);
        }
        private SwagLyricsBridge _bridge = new SwagLyricsBridge();

        public MainWindowViewModel()
        {
            Current = this;
            LoadThemes();
            _bridge.GetLyrics();
        }

        private void LoadThemes()
        {
            var dark = new StyleInclude(new Uri("resm:Styles?assembly=SwagLyricsGUI"))
            {
                Source = new Uri("resm:Avalonia.Themes.Default.Accents.BaseDark.xaml?assembly=Avalonia.Themes.Default")
            };
            var light = new StyleInclude(new Uri("resm:Styles?assembly=SwagLyricsGUI"))
            {
                Source = new Uri("resm:Avalonia.Themes.Default.Accents.BaseLight.xaml?assembly=Avalonia.Themes.Default")
            };

            Themes = new List<IStyle>()
            {
               light.Loaded,
                dark.Loaded,
                Application.Current.Styles[0]

            };
        }

        private void ChangeTheme(int index)
        {
            if (index > Themes.Count - 1 || index < 0) return;
            if (MainWindow.Current.Styles.Count == 0)
            {
                MainWindow.Current.Styles.Add(Themes[index]);
            }
            else
            {
                MainWindow.Current.Styles[0] = Themes[index];
            }
        }
    }
}
