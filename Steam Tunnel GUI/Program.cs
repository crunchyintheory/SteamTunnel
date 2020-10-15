using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamTunnel.GUI
{
    static class Program
    {
        public static string IniPath { get; private set; }

        public static Dictionary<string, string> Options = new Dictionary<string, string>();
        public static string ConfigDir;

        public static string Version = "1.0.0b1";

        public static void LoadSettings()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            ConfigDir = Path.Combine(appdata, "Steam Tunnel");
            Directory.CreateDirectory(ConfigDir);
            IniPath = Path.Combine(ConfigDir, "SteamTunnel.ini");

            if (File.Exists(IniPath))
            {
                string[] lines = File.ReadAllLines(IniPath);
                char[] divider = { '=' };

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(divider, 2);

                    if (parts.Length > 1)
                    {
                        Options.Add(parts[0], parts[1]);
                    }
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LoadSettings();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
