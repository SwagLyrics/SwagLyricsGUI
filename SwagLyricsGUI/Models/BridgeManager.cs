using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SwagLyricsGUI.Models
{
    public class BridgeManager
    {
        public static string BridgeFilesPath { get; private set; } = "";
        public Assembly ExecutingAssembly { get; set; }
        public string TempFolderPath { get; set; }
        public const string TempDirectoryName = "SwagLyricsGUI";
        public BridgeManager() 
        { 
            ExecutingAssembly = Assembly.GetExecutingAssembly();
            TempFolderPath = Path.GetTempPath();
        }

        public void InitTempDirectory()
        {
            Directory.CreateDirectory(Path.Join(TempFolderPath, TempDirectoryName));
        }

        public void LoadEmbeddedScriptsToTempFolder()
        {
            BridgeFilesPath = Path.Combine(TempFolderPath, TempDirectoryName);
            LoadEmbeddedScript("swaglyrics_api_bridge.py");
            LoadEmbeddedScript("swaglyrics_installer.py");
        }

        private void LoadEmbeddedScript(string scriptName)
        {
            if (File.Exists(Path.Join(BridgeFilesPath, scriptName))) return;

                string resourceName = ExecutingAssembly.GetManifestResourceNames()
            .Single(str => str.EndsWith(scriptName));

            using (Stream stream = ExecutingAssembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();          
                File.WriteAllText(Path.Join(BridgeFilesPath, scriptName), result);
            }
        }
    }
}
