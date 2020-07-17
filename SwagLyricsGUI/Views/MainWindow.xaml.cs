using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SwagLyricsGUI.Views
{
    public class MainWindow : Window
    {
        public static MainWindow Current { get; set; }
        private ScrollViewer sv;
        public double ScrollViewerVieportHeight => sv.Viewport.Height;
        public MainWindow()
        {
            InitializeComponent();
            Current = this;
#if DEBUG
            this.AttachDevTools();
#endif
            sv = this.Find<ScrollViewer>("scrollViewer");
            
            MaxHeight = Screens.Primary.WorkingArea.Height;
            Height = Screens.Primary.WorkingArea.Height - 80;
            Position = new PixelPoint(Screens.Primary.WorkingArea.Right - (int)Width - 15,
    Screens.Primary.WorkingArea.Bottom - (int)Height - 30);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
