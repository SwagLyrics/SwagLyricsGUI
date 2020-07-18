using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Serilog.Parsing;

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
            MaxHeight = Screens.Primary.Bounds.Height;
            Height = Screens.Primary.WorkingArea.Height - 80;
            Position = new PixelPoint(Screens.Primary.Bounds.Right - (int)Width - 15, 0);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
