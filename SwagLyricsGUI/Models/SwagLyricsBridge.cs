using SwagLyricsGUI.ViewModels;
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
            if (e.Data != null && e.Data.StartsWith("Getting lyrics for")) MainWindowViewModel.Current.Lyrics = "";
            if (e.Data?.ToLower() == "(press ctrl+c to quit)") return;
            MainWindowViewModel.Current.Lyrics += $"\n{e.Data}";
        }
    }
}
