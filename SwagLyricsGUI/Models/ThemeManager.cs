using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using SwagLyricsGUI.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwagLyricsGUI.Models
{
    public class ThemeManager
    {
        public List<IStyle> Themes { get; set; }

        public void LoadThemes()
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

        public void ChangeTheme(int index)
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
