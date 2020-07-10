using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using SwagLyricsGUI.ViewModels;
using System;
using System.Collections.Generic;

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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
