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
        public MainWindow()
        {
            InitializeComponent();
            Current = this;
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
