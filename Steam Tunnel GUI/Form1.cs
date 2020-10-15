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
using System.Runtime.Serialization.Formatters.Binary;

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
            ColumnHeader header1 = new ColumnHeader
            {
                Text = "",
                Name = "col1",
                Width = listView1.Width - 4 - SystemInformation.VerticalScrollBarWidth
            };
            listView1.Columns.Add(header1);
            listView1.HeaderStyle = ColumnHeaderStyle.None;

            listView2.View = View.Details;
            listView2.SmallImageList = il2;
            listView2.Scrollable = true;
            ColumnHeader header2 = new ColumnHeader
            {
                Text = "",
                Name = "col2",
                Width = listView2.Width - 4 - SystemInformation.VerticalScrollBarWidth
            };
            listView2.Columns.Add(header2);
            listView2.HeaderStyle = ColumnHeaderStyle.None;

            Tunnel.FileCopyProgress += (filesCopied, totalFiles) =>
            {
                resetProgressBar(filesCopied, (int)totalFiles, 1, "Copying Files...  " + filesCopied.ToString() + "/" + totalFiles.ToString());
            };

            Text += " v" + Program.Version;

            // ------------------------

            bool steamOpen = System.Diagnostics.Process.GetProcessesByName("steam").Length > 0;

            SteamOpenWarningForm settings = new SteamOpenWarningForm();
            settings.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = textBox1.Text,
                ShowNewFolderButton = false,
                Description = "Select either \"Steam\", \"Steam.exe\", \"SteamApps\", or \"common\""
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                SelectedPath = textBox2.Text,
                ShowNewFolderButton = false,
                Description = "Select either \"Steam\", \"Steam.exe\", \"SteamApps\", or \"common\""
            };
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
            resetProgressBar(0, 200, 100, "Searching Directory...");
            updateProgressBar(null, true);
            await Task.Delay(600);
            try
            {
                Path.GetFullPath(textBox1.Text);
                listView1.Items.Clear();
                listView1.games.Clear();
                il1.Images.Clear();

                string t = Tunnel.validateSteamDirectory(textBox1.Text);

                if (t != null)
                {
                    sourceDir = t;
                    await updateList(listView1, il1, sourceDir);
                }
                else
                {
                    resetProgressBar(0, 0, 0, "Failed.");
                    string message = "The path is not a valid Steam directory";
                    string caption = "Error: Invalid Path";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;

                    MessageBox.Show(message, caption, buttons, icon);
                }
            } catch (Exception error)
            {
                resetProgressBar(0, 0, 0, "Failed.");
                string message = "The path could not be resolved";
                string caption = "Error: Invalid Path";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBoxIcon icon = MessageBoxIcon.Error;

                Console.WriteLine(error.Message);

                MessageBox.Show(message, caption, buttons, icon);
            }
        }

        private async void refresh2_Click(object sender = null, EventArgs e = null)
        {
            resetProgressBar(0, 200, 100, "Searching Directory...");
            updateProgressBar(null, true);
            await Task.Delay(600);
            try
            {
                Path.GetFullPath(textBox2.Text);
                listView2.Items.Clear();
                listView2.games.Clear();
                il2.Images.Clear();

                string t = Tunnel.validateSteamDirectory(textBox2.Text);

                if (t != null)
                {
                    destDir = t;
                    await updateList(listView2, il2, destDir);
                }
                else
                {
                    resetProgressBar(0, 0, 0, "Failed.");
                    string message = "The path is not a valid Steam directory";
                    string caption = "Error: Invalid Path";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;

                    MessageBox.Show(message, caption, buttons, icon);
                }
            }
            catch (Exception)
            {
                resetProgressBar(0, 0, 0, "Failed.");
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
            List<Game> gameList = new List<Game>(fileArray.Length);
            foreach(string file in fileArray)
            {
                gameList.Add(Tunnel.getGameInfo(file));
            }
            /*for (int i = 0; i < fileArray.Length; i++)
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
            }*/
            gameList.Sort((x, y) => string.Compare(x.name, y.name));
            list.games = gameList;

            Dictionary<string, Icon> iconCache = LoadIconsCache();
            bool iconCacheUpdated = false;
            Icon icon;
            _ = Program.Options.TryGetValue("Icons", out string useIcons);

            for (int i = 0; i < gameList.Count; i++)
            {
                if (useIcons != "0")
                {
                    if(!iconCache.TryGetValue(gameList[i].appId, out icon))
                    {
                        icon = await Task.Run(() => gameList[i].icon(dir + "\\common"));
                        if (icon == null)
                        {
                            icon = SystemIcons.Application;
                        }
                        else
                        {
                            iconCache.Add(gameList[i].appId, icon);
                            iconCacheUpdated = true;
                        }
                    }
                } else
                {
                    icon = SystemIcons.Application;
                }
                imageList.Images.Add(gameList[i].appId, icon);
                ListViewItem lvi = new ListViewItem
                {
                    ImageIndex = i,
                    Text = "  " + gameList[i].name
                };
                list.Items.Add(lvi);
                icon = null;
            }

            if(iconCacheUpdated)
            {
                this.SaveIconCache(iconCache);
            }

            list.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.None);
            updateProgressBar("Done", true);
        }

        private void openSettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm();
            settings.ShowDialog();
        }

        private Dictionary<string, Icon> LoadIconsCache()
        {
            string path = Path.Combine(Program.ConfigDir, "icons.bin");
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    return (Dictionary<string, Icon>) bf.Deserialize(fs);
                } catch (Exception)
                {
                    Console.WriteLine("Icons cache corrupted, purging...");
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Steam Tunnel Bizarre Adventures Part 4: Icon Cache is Unbreakable");
                    }
                }
            }
            return new Dictionary<string, Icon>();
        }

        private void SaveIconCache(Dictionary<string, Icon> cache)
        {
            try
            {
                FileStream fs = new FileStream(Path.Combine(Program.ConfigDir, "icons.bin"), FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, cache);
                fs.Close();
            } catch (Exception)
            {
                Console.WriteLine("Icons cache is unwritable.");
            }
        }
    }
    public class GameListView : ListView
    {
        public List<Game> games = new List<Game>();
    }
}
