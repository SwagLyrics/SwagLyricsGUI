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
            Height = Screens.Primary.WorkingArea.Height - 40;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
