using SwagLyricsGUI.ViewModels;
using SwagLyricsGUI.Views;
using System;
using System.Diagnostics;

namespace SwagLyricsGUI.Models
{
    public class SwagLyricsBridge
    {
        public void GetLyrics()
        {
            var cmd = "-c";
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "swaglyrics",
                    Arguments = cmd,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };
            process.OutputDataReceived += Process_OutputDataReceived;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            Console.Read();
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;
            if (e.Data.StartsWith("Getting lyrics for"))
            {
                MainWindowViewModel.Current.Lyrics = "";
                MainWindowViewModel.Current.Song = e.Data.Split("for")[1].Replace(".", "");
            }
            else
            {
                if (string.Compare(e.Data, "(Press Ctrl+C to quit)", true) == 0) return;
                MainWindowViewModel.Current.Lyrics += $"\n{e.Data}";
            }
        }
    }
}
