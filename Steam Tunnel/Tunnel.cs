using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Net;

namespace SteamTunnel
{
    public class Tunnel
    {
        public delegate void IntLongDelegate(int Int1, long Long);
        public static event IntLongDelegate FileCopyProgress;
        public static int filesCopied;
        public static long totalFiles;

        public static string validateSteamDirectory(string path)
        {
            if (Path.GetFileName(path) == "Steam" &&
                    Directory.Exists(path) &&
                    Directory.GetFiles(path).Where(x =>
                        Regex.IsMatch(
                            Path.GetFileName(x),
                            "steam.dll",
                            RegexOptions.IgnoreCase))
                    .Count() > 0)
            {
                return path + "\\SteamApps";
            }
            else if (Path.GetFileName(path) == "SteamApps" &&
              Directory.Exists(path) &&
              Directory.GetFiles(Directory.GetParent(path).FullName).Where(x =>
                  Regex.IsMatch(
                      Path.GetFileName(x),
                      "steam.dll",
                      RegexOptions.IgnoreCase))
              .Count() > 0)
            {
                return path;
            }
            else if (Path.GetFileName(path) == "common" &&
              Directory.Exists(path) &&
              Directory.GetFiles(Directory.GetParent(path).Parent.FullName).Where(x =>
              Regex.IsMatch(
                  Path.GetFileName(x),
                  "steam.dll",
                  RegexOptions.IgnoreCase))
              .Count() > 0)
            {
                return Directory.GetParent(path).FullName;
            }
            else
            {
                return null;
            }
        }

        public static Game getGameInfo(string path)
        {
            StreamReader stream = new StreamReader(File.OpenRead(path));
            string line;
            Game game = new Game();
            game.manifestPath = path;
            while ((line = stream.ReadLine()) != null)
            {
                string[] items = line.Split('\t');
                for(int i = 0; i < items.Length; i++)
                {
                    if(items[i].StartsWith("\"appid\""))
                    {
                        game.appId = items[i + 2].Replace("\"", "");
                        game.workshopRelativePath = "\\workshop\\content\\" + game.appId;
                        game.workshopManifestRelativePath = "\\workshop\\appworkshop_" + game.appId + ".acf";
                    }
                    else if(items[i].StartsWith("\"name\""))
                    {
                        game.name = items[i + 2].Replace("\"", "");
                    }
                    else if(items[i].StartsWith("\"installdir\""))
                    {
                        game.installDir = items[i + 2].Replace("\"", "");
                    }
                }
            }
            stream.Close();
            return game;
        }
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                CopyProgress(++filesCopied, totalFiles);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
        internal static void CopyProgress(int filesCopied, long totalFiles)
        {
            Tunnel.filesCopied = filesCopied;
            Tunnel.totalFiles = totalFiles;
            FileCopyProgress?.Invoke(filesCopied, totalFiles);
        }
    }
    public class AppStates
    {
        public Game AppState;
    }
    public class Game
    {
        public string appId;
        public string name;
        public string installDir;
        public string manifestPath;
        public string workshopRelativePath;
        public string workshopManifestRelativePath;

        public async Task<Icon> icon(string commonPath)
        {
            string[] fileArray = Directory.GetFiles(commonPath + "\\" + installDir, "*.exe", SearchOption.AllDirectories);
            foreach(string file in fileArray)
            {
                Icon icon = Icon.ExtractAssociatedIcon(file);
                if (!Regex.IsMatch(file, ".+(?:redist|setup|core|extensions|loader|api|physx|install|awesomium|reporter|runme|launcher|dosbox|asset|server|config|update|dotnet).+", RegexOptions.IgnoreCase)) {
                    return icon;
                }
            }
            return null;
        }

        public async Task<WorkshopFileData> getWorkshopInfo(string dir)
        {
            WorkshopFileData fileData = new WorkshopFileData();
            fileData.isManifestPresent = File.Exists(dir + workshopManifestRelativePath);
            fileData.isDirectoryPresent = Directory.Exists(dir + workshopRelativePath);
            return fileData;
        }

        public async Task moveWorkshopContent(string sourceDir, string destDir, WorkshopFileData fileData)
        {
            if (fileData.isDirectoryPresent)
            {
                Tunnel.CopyProgress(0, Directory.GetFiles(sourceDir + workshopRelativePath, "*", SearchOption.AllDirectories).LongLength);
                Tunnel.CopyAll(new DirectoryInfo(sourceDir + workshopRelativePath), new DirectoryInfo(destDir + workshopRelativePath));
                Directory.Delete(sourceDir + workshopRelativePath, true);
            }
            if (fileData.isManifestPresent)
            {
                File.Copy(sourceDir + workshopManifestRelativePath, destDir + workshopManifestRelativePath);
                File.Delete(sourceDir + workshopManifestRelativePath);
            }
        }

        public async Task moveGame(string sourceDir, string destDir)
        {
            if (Directory.Exists(sourceDir + "\\common\\" + installDir))
            {
                Tunnel.CopyProgress(0, Directory.GetFiles(sourceDir + "\\common\\" + installDir, "*", SearchOption.AllDirectories).LongLength);
                Tunnel.CopyAll(new DirectoryInfo(sourceDir + "\\common\\" + installDir), new DirectoryInfo(destDir + "\\common\\" + installDir));
                Directory.Delete(sourceDir + "\\common\\" + installDir, true);
            }
            File.Copy(manifestPath, destDir + "\\" + Path.GetFileName(manifestPath));
            File.Delete(manifestPath);
        }
    }
    public struct WorkshopFileData
    {
        public bool isManifestPresent, isDirectoryPresent;
    }
}
