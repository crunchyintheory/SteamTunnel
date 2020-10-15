using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamTunnel.GUI
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            _ = Program.Options.TryGetValue("Icons", out string icons);

            if (icons != "0")
            {
                showIconsBox.Checked = true;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            FileStream file = File.Open(Program.IniPath, FileMode.Create);

            SettingsWrite(file, "Icons=" + (showIconsBox.Checked ? 1 : 0));

            Program.LoadSettings();
        }

        private void SettingsWrite(FileStream stream, string data)
        {
            byte[] dataBytes = new UTF8Encoding(true).GetBytes(data + Environment.NewLine);
            stream.Write(dataBytes, 0, dataBytes.Length);
        }
    }
}
