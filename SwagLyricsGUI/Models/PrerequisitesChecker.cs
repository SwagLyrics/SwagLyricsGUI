using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SwagLyricsGUI.Models
{
    public class PrerequisitesChecker
    {
        public static string PythonCmdPostFix { get; set; } = "";
        public bool SupportedPythonVersionInstalled()
        {
            return CheckPythonInstalled("");
        }

        public void InstallSwagLyricsIfMissing()
        {
            string path = Path.Join(BridgeManager.BridgeFilesPath, "swaglyrics_installer.py");
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = $"python{PythonCmdPostFix}",
                Arguments = path,
                UseShellExecute = false,
            };
            Process process = new Process() { StartInfo = start };
            process.Start();
            process.WaitForExit();          
        }

        private bool CheckPythonInstalled(string postfix)
        {
            bool result = false;
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = $"python{postfix}",
                Arguments = "--version",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(start))
            {                
                using (StreamReader reader = process.StandardError)
                {
                    var res = reader.ReadToEnd();
                    if (res != "" && postfix != "")
                    {
                        return false;
                    }
                }
                using (StreamReader reader = process.StandardOutput)
                {
                    var res = reader.ReadToEnd();
                    if (res == "")
                    {
                        result = false;
                    }
                    else
                    {
                        var version = res.Split(" ")[1].Split('.');
                        int ver0 = int.Parse(version[0]);
                        result = (ver0 == 3 && int.Parse(version[1]) >= 6) || ver0 > 3;
                    }
                }

                if(!result && postfix == "")
                {
                    result = CheckPythonInstalled("3");
                    if (result)
                        PythonCmdPostFix = "3";
                }
            }
            return result;
        }
    }
}
