using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Serilog.Parsing;
using SwagLyricsGUI.ViewModels;
using System.Configuration;

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
            this.Closing += MainWindow_Closing;
            this.Opened += MainWindow_Opened;
            Current = this;
#if DEBUG
            this.AttachDevTools();
#endif
            sv = this.Find<ScrollViewer>("scrollViewer");
        }

        private void MainWindow_Opened(object sender, System.EventArgs e)
        {
            MaxHeight = Screens.Primary.Bounds.Height;
            Height = Screens.Primary.Bounds.Height - 100;
            SetPosition();
        }

        private void SetPosition()
        {
            string x = MainWindowViewModel.Current.Config.AppSettings.Settings["PosX"].Value;
            string y = MainWindowViewModel.Current.Config.AppSettings.Settings["PosY"].Value;
            if(x != "defualt" && y != "default")
            {
                Position = new PixelPoint(int.Parse(x), int.Parse(y));
            }
            else
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
                Position = new PixelPoint(Position.X, 0);
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var config = MainWindowViewModel.Current.Config;
            config.AppSettings.Settings["PosX"].Value = Position.X.ToString();
            MainWindowViewModel.Current.Config.AppSettings.Settings["PosY"].Value = Position.Y.ToString();
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
