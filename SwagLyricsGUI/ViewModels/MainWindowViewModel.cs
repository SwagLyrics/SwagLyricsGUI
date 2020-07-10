using Avalonia;
using ReactiveUI;
using SwagLyricsGUI.Models;
using SwagLyricsGUI.Views;

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
        private double _scrollSpeed = 1;
        private double t = 0;


        private int _themeIndex = 1;
        public int ThemeIndex
        {
            get => _themeIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _themeIndex, value);
                ThemeManager.ChangeTheme(value);
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
                if(AutoScroll)
                    StartAutoScroll();
            }
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
            ThemeManager = new ThemeManager();
            ThemeManager.LoadThemes();
            _bridge.GetLyrics();
            _bridge.OnNewSong += _bridge_OnNewSong;
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _bridge_OnNewSong(object sender, System.EventArgs e)
        {
            ScrollBarOffset = new Vector(0, 0);
            t = 0;
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AutoScroll)
            {
                ScrollBarOffset = new Vector(0, MathEx.Lerp(0, MainWindow.Current.ScrollViewerVieportHeight, t));
            }
            t += _scrollSpeed * 0.0001;
        }

        private void StartAutoScroll()
        {
            if (!_timer.Enabled)
            {
                _timer.Start();
            }

        }
    }
}
