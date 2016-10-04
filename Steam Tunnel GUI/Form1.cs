using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Trileans;

namespace SteamTunnel.GUI
{
    delegate void updateProgressBarCallback(string caption, bool step);
    delegate void resetProgressBarCallback(int Value, int Maximum, int Step, string caption);
    public partial class Form1 : Form
    {
        string sourceDir;
        string destDir;
        ImageList il1;
        ImageList il2;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            il1 = new ImageList();
            il2 = new ImageList();

            listView1.View = View.Details;
            listView1.SmallImageList = il1;
            listView1.Scrollable = true;
            ColumnHeader header1 = new ColumnHeader();
            header1.Text = "";
            header1.Name = "col1";
            header1.Width = listView1.Width - 4 - SystemInformation.VerticalScrollBarWidth;
            listView1.Columns.Add(header1);
            listView1.HeaderStyle = ColumnHeaderStyle.None;

            listView2.View = View.Details;
            listView2.SmallImageList = il2;
            listView2.Scrollable = true;
            ColumnHeader header2 = new ColumnHeader();
            header2.Text = "";
            header2.Name = "col2";
            header2.Width = listView2.Width - 4 - SystemInformation.VerticalScrollBarWidth;
            listView2.Columns.Add(header2);
            listView2.HeaderStyle = ColumnHeaderStyle.None;

            Tunnel.FileCopyProgress += (filesCopied, totalFiles) =>
            {
                resetProgressBar(filesCopied, (int)totalFiles, 1, "Copying Files...  " + filesCopied.ToString() + "/" + totalFiles.ToString());
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = textBox1.Text;
            dialog.ShowNewFolderButton = false;
            dialog.Description = "Select either \"Steam\", \"Steam.exe\", \"SteamApps\", or \"common\"";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = textBox2.Text;
            dialog.ShowNewFolderButton = false;
            dialog.Description = "Select either \"Steam\", \"Steam.exe\", \"SteamApps\", or \"common\"";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dialog.SelectedPath;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0)
            {
                refresh1.Enabled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                refresh2.Enabled = true;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(listView1.SelectedItems.Count)
            {
                case 1:
                    this.moveToDestButton.Enabled = true;
                    listView2.SelectedIndices.Clear();
                    break;
                default:
                    this.moveToDestButton.Enabled = false;
                    break;
            }
            moveBackButton.Enabled = false;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (listView2.SelectedItems.Count)
            {
                case 1:
                    this.moveBackButton.Enabled = true;
                    listView1.SelectedIndices.Clear();
                    break;
                default:
                    this.moveBackButton.Enabled = false;
                    break;
            }
            moveToDestButton.Enabled = false;
        }

        private async void refresh1_Click(object sender = null, EventArgs e = null)
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = 200;
            progressBar1.Step = 100;
            updateProgressBar("Searching Directory...");
            await Task.Delay(600);
            try
            {
                Path.GetFullPath(textBox1.Text);
                listView1.Items.Clear();
                listView1.games.Clear();
                il1.Images.Clear();

                trilean t = Tunnel.validateSteamDirectory(textBox1.Text);

                if (t == true)
                {
                    sourceDir = (string)t.embedded;
                    await updateList(listView1, il1, sourceDir);
                }
                else
                {
                    string message = "The path is not a valid Steam directory";
                    string caption = "Error: Invalid Path";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;

                    MessageBox.Show(message, caption, buttons, icon);
                }
            } catch (Exception)
            {
                string message = "The path could not be resolved";
                string caption = "Error: Invalid Path";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;

                MessageBox.Show(message, caption, buttons, icon);
            }
        }

        private async void refresh2_Click(object sender = null, EventArgs e = null)
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = 200;
            progressBar1.Step = 100;
            updateProgressBar("Searching Directory...");
            await Task.Delay(600);
            try
            {
                Path.GetFullPath(textBox2.Text);
                listView2.Items.Clear();
                listView2.games.Clear();
                il2.Images.Clear();

                trilean t = Tunnel.validateSteamDirectory(textBox2.Text);

                if (t == true)
                {
                    destDir = (string)t.embedded;
                    await updateList(listView2, il2, destDir);
                }
                else
                {
                    progressBar1.Value = 0;
                    string message = "The path is not a valid Steam directory";
                    string caption = "Error: Invalid Path";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;

                    MessageBox.Show(message, caption, buttons, icon);
                }
            }
            catch (Exception)
            {
                progressBar1.Value = 0;
                string message = "The path could not be resolved";
                string caption = "Error: Invalid Path";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;

                MessageBox.Show(message, caption, buttons, icon);
            }
        }

        private async void moveToDestButton_Click(object sender, EventArgs e)
        {
            List<Game> games = listView1.games.Where(x => x.installDir == listView1.games[listView1.SelectedIndices[0]].installDir).ToList();
            listView1.SelectedIndices.Clear();
            foreach (Game game in games) {
                resetProgressBar(0, 100, 1, "Copying Files...");
                await Task.Run(() => game.moveGame(sourceDir, destDir));
                resetProgressBar(0, 100, 1, "Copying Files...");
                await Task.Run(async () => game.moveWorkshopContent(destDir, sourceDir, await game.getWorkshopInfo(sourceDir)));
            }
            refresh1_Click();
            refresh2_Click();
            updateProgressBar("Done", false);
        }

        private async void moveBackButton_Click(object sender, EventArgs e)
        {
            List<Game> games = listView2.games.Where(x => x.installDir == listView2.games[listView2.SelectedIndices[0]].installDir).ToList();
            listView2.SelectedIndices.Clear();
            foreach (Game game in games) {
                resetProgressBar(0, 100, 1, "Copying Files...");
                await Task.Run(() => game.moveGame(destDir, sourceDir));
                resetProgressBar(0, 100, 1, "Copying Files...");
                await Task.Run(async () => game.moveWorkshopContent(destDir, sourceDir, await game.getWorkshopInfo(destDir)));
            }
            refresh2_Click();
            refresh1_Click();
            updateProgressBar("Done", false);
        }
        public void updateProgressBar(string caption = null, bool step = true)
        {
            if (!label3.InvokeRequired && !progressBar1.InvokeRequired)
            {
                if (caption != null)
                {
                    label3.Text = caption;
                }
                if (step)
                {
                    progressBar1.PerformStep();
                }
            }
            else
            {
                updateProgressBarCallback d = new updateProgressBarCallback(updateProgressBar);
                Invoke(d, new object[] { caption, step });
            }
        }
        public void resetProgressBar(int Value, int Maximum, int Step, string caption = "Done")
        {
            if (!label3.InvokeRequired && !progressBar1.InvokeRequired)
            {
                label3.Text = caption;
                progressBar1.Value = Value;
                progressBar1.Maximum = Maximum;
                progressBar1.Step = Step;
            }
            else
            {
                resetProgressBarCallback d = new resetProgressBarCallback(resetProgressBar);
                Invoke(d, new object[] { Value, Maximum, Step, caption });
            }
        }
        public async Task updateList(GameListView list, ImageList imageList, string dir)
        {
            string[] fileArray = Directory.GetFiles(dir).Where(x => Regex.IsMatch(Path.GetFileName(x), @"appmanifest_\d+\.acf")).ToArray();
            for (int i = 0; i < fileArray.Length; i++)
            {
                string file = fileArray[i];
                Game game = Tunnel.getGameInfo(file);
                list.games.Add(game);
                Icon icon = await Task.Run(() => game.icon(dir + "\\common"));
                imageList.Images.Add(game.appId, icon);
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = i;
                lvi.Text = "  " + game.name;
                list.Items.Add(lvi);
            }
            list.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.None);
            updateProgressBar("Done");
        }
    }
    public class GameListView : ListView
    {
        public List<Game> games = new List<Game>();
    }
}
