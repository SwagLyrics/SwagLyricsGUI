using Avalonia;
using ReactiveUI;
using SwagLyricsGUI.Models;
using SwagLyricsGUI.Views;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SwagLyricsGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {        
        public ICommand CloseAppCommand { get; set; }
        public static MainWindowViewModel Current { get; set; }
        public ThemeManager ThemeManager { get; set; }
        System.Timers.Timer _timer = new System.Timers.Timer(20)
        {
            AutoReset = true,            
        };
        System.Timers.Timer _loadingTimer = new System.Timers.Timer(100)
        {
            AutoReset = true,
        };
        public char[] LoadingASCII;
        private int _lastLoadingIndex = 0;
        public double ScrollSpeed = 1.1;
        private double t = 0;


        private int _themeIndex = 2;
        public int ThemeIndex
        {
            get => _themeIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _themeIndex, value);
                ThemeManager.ChangeTheme(value);           
                Config.AppSettings.Settings["Theme"].Value = $"{value}";
                Config.Save();
                ConfigurationManager.RefreshSection("appSettings");
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

        private Avalonia.Layout.HorizontalAlignment _lyricsAlignment = Avalonia.Layout.HorizontalAlignment.Left;
        public Avalonia.Layout.HorizontalAlignment LyricsAlignment
        {
            get => _lyricsAlignment;
            set => this.RaiseAndSetIfChanged(ref _lyricsAlignment, value);
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
        public BridgeManager Manager = new BridgeManager();

        public MainWindowViewModel()
        {
            CloseAppCommand = new Command(OnClose);
            bool pythonInstalled = checker.SupportedPythonVersionInstalled();
            if (pythonInstalled)
            {
                Manager.InitTempDirectory();
                Manager.LoadEmbeddedScriptsToTempFolder();
                checker.InstallSwagLyricsIfMissing();
                Current = this;
                ThemeManager = new ThemeManager();
                ThemeManager.LoadThemes();
                Bridge.StartLyricsBridge();
                Bridge.OnNewSong += Bridge_OnNewSong;
                Bridge.OnLyricsLoaded += Bridge_OnLyricsLoaded;
                Bridge.OnError += Bridge_OnError;
                Bridge.OnResumed += Bridge_OnResumed;
                Bridge.OnAdvertisement += Bridge_OnAdvertisement;
                _timer.Elapsed += _timer_Elapsed;
                _loadingTimer.Elapsed += _loadingTimer_Elapsed;
                LoadingASCII = "|/-\\".ToCharArray();

                Config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
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

        private void Bridge_OnAdvertisement(object sender, EventArgs e)
        {
            System.Timers.Timer factTimer = new System.Timers.Timer(15000)
            {
                AutoReset = true
            };            
            Song = "Advertisement time. Catch some fun facts";
            Lyrics = RandomFactsFetcher.GetRandomFact().ToString();
            factTimer.Elapsed += (s, e) =>
            {
                if (!Bridge.IsAdvertisement) { factTimer.Stop(); }
                else { Lyrics = RandomFactsFetcher.GetRandomFact().ToString(); }
            };
            factTimer.Start();
        }

        private void _loadingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Lyrics = LoadingASCII[_lastLoadingIndex].ToString();
            if (_lastLoadingIndex == LoadingASCII.Length - 1) _lastLoadingIndex = -1;
            _lastLoadingIndex++;
        }

        private void OnClose(object obj)
        {
            Bridge?.LyricsProcess.Kill(true);
            File.Delete(Bridge.BridgeFileOnPath);
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
            LyricsAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            _loadingTimer.Stop();
            Lyrics = e.Lyrics;
            Task.Delay(20000).ContinueWith((task) => { _timer.Start(); });
        }

        private void Bridge_OnNewSong(object sender, NewSongEventArgs e)
        {
            LyricsAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            Song = e.Song;
            _loadingTimer.Start();
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
