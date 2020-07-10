using SwagLyricsGUI.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SwagLyricsGUI.Models
{
    public class SwagLyricsBridge
    {
        public event EventHandler OnNewSong;
     
        public void GetLyrics()
        {
            var cmd = Path.Combine(Directory.GetCurrentDirectory(), Path.Join("..", "..", "..", "Models", "SwagLyrics_api_bridge.py"));
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = cmd,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                },
                EnableRaisingEvents = true
            };
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            Console.Read();
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if(e.Data != null)
            {
                MainWindowViewModel.Current.Lyrics = e.Data;
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;
            string data = DecodeFrom64(e.Data);
            if (data.EndsWith(':') && data.Contains("by"))
            {
                MainWindowViewModel.Current.Song = data.Split(":")[0];
                MainWindowViewModel.Current.Lyrics = "Loading lyrics...";

                OnNewSong?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MainWindowViewModel.Current.Lyrics = $"\n{data}\n"; // \n are "Margins"
            }

        }

        public static string DecodeFrom64(string input)
        {
            input = input.Replace("b'", "");
            input = input.Replace("'", "");
            byte[] encodedDataAsBytes =
            System.Convert.FromBase64String(input);
            string returnValue =
            System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
            return returnValue;
        }

    }
}
