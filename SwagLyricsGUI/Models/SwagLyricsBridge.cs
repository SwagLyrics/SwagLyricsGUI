using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SwagLyricsGUI.Models
{
    public class SwagLyricsBridge
    {
        public event EventHandler<NewSongEventArgs> OnNewSong;
        public event EventHandler<LyricsLoadedEventArgs> OnLyricsLoaded;
        public event EventHandler<LyricsLoadedEventArgs> OnError;
        public event EventHandler OnResumed;

        public void GetLyrics()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), Path.Join("Bridge","SwagLyrics_api_bridge.py"));
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = $"python{PrerequisitesChecker.PythonCmdPostFix}",
                    Arguments = path,
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
            if (e.Data == null) return;
            OnError?.Invoke(this, new LyricsLoadedEventArgs(e.Data));
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) return;
            string data = DecodeFrom64(e.Data);
            if (data.EndsWith(':') && data.Contains("by"))
            {
                string song = data.Split(":")[0];

                OnNewSong?.Invoke(this, new NewSongEventArgs(song));
            }
            else if(data == "Resumed")
            {
                OnResumed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnLyricsLoaded?.Invoke(this, new LyricsLoadedEventArgs($"\n{data}\n")); // \n are "Margins"
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
