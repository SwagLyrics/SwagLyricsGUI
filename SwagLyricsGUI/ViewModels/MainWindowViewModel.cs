using Avalonia;
using ReactiveUI;
using SwagLyricsGUI.Models;
using SwagLyricsGUI.Views;
using System.Configuration;
using System.Threading.Tasks;

namespace SwagLyricsGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static MainWindowViewModel Current { get; set; }
        public ThemeManager ThemeManager { get; set; }
        System.Timers.Timer _timer = new System.Timers.Timer(20)
        {
            AutoReset = true,            
        };
        public double ScrollSpeed = 1.1;
        private double t = 0;


        private int _themeIndex = 1;
        public int ThemeIndex
        {
            get => _themeIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _themeIndex, value);
                ThemeManager.ChangeTheme(value);
                Config.AppSettings.Settings["Theme"].Value = $"{value}";
                Config.Save();
            }
        }

        private bool _autoScroll = true;
        public bool AutoScroll
        {
            get => _autoScroll;
            set
            {
                _autoScroll = value;
                this.RaisePropertyChanged("AutoScroll");
            }
        }

        private Vector _scrollBarOffset;

        public Vector ScrollBarOffset
        {
            get => _scrollBarOffset;
            set => this.RaiseAndSetIfChanged(ref _scrollBarOffset, value);
        }


        private string _lyrics = "";
        public string Lyrics
        {
            get => _lyrics;
            set 
            { 
                this.RaiseAndSetIfChanged(ref _lyrics, value);
            }
        }

        private string _song = "Nothing is playing";
        public string Song
        {
            get => _song;
            set => this.RaiseAndSetIfChanged(ref _song, value);
        }
        public SwagLyricsBridge Bridge = new SwagLyricsBridge();
        public Configuration Config;
        PrerequisitesChecker checker = new PrerequisitesChecker();

        public MainWindowViewModel()
        {
            bool pythonInstalled = checker.SupportedPythonVersionInstalled();
            if (pythonInstalled)
            {
                checker.InstallSwagLyricsIfMissing();
                Current = this;
                ThemeManager = new ThemeManager();
                ThemeManager.LoadThemes();
                Bridge.GetLyrics();
                Bridge.OnNewSong += Bridge_OnNewSong;
                Bridge.OnLyricsLoaded += Bridge_OnLyricsLoaded;
                Bridge.OnError += Bridge_OnError;
                Bridge.OnResumed += Bridge_OnResumed;
                _timer.Elapsed += _timer_Elapsed;

                Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (Config.AppSettings.Settings["Theme"]?.Value is string theme)
                {
                    ThemeIndex = int.Parse(theme);
                }
            }
            else
            {
                Song = "Unsupported Python Version!";
                Lyrics = "Minimal supported version is Python 3.6.\nDownload at " + @"https://www.python.org/downloads/";
            }
        }

        private void Bridge_OnResumed(object sender, System.EventArgs e)
        {
            _timer.Start();
        }

        private void Bridge_OnError(object sender, LyricsLoadedEventArgs e)
        {
            if (e.Lyrics == "SpotifyNotRunning")
            {
                _timer.Stop();
            }
            else
            {
                Lyrics = e.Lyrics;
            }
        }

        private void Bridge_OnLyricsLoaded(object sender, LyricsLoadedEventArgs e)
        {
            Lyrics = e.Lyrics;
            Task.Delay(20000).ContinueWith((task) => { _timer.Start(); });
        }

        private void Bridge_OnNewSong(object sender, NewSongEventArgs e)
        {
            Song = e.Song;
            Lyrics = "Loading lyrics...";
            ScrollBarOffset = new Vector(0, 0);
            t = 0;
            _timer.Stop();
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AutoScroll)
            {
                ScrollBarOffset = new Vector(0, MathEx.Lerp(0, MainWindow.Current.ScrollViewerVieportHeight, t));
            }
            t += ScrollSpeed * 0.0001;
        }
    }
}
