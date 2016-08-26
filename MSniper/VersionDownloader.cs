using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSniper
{
    public static class VersionDownloader
    {
        private static byte[] GetFile(string fileVersion)
        {
            try
            {
                string url = string.Format(FConfig.FileLink, fileVersion);
                using (MSniperClient w = new MSniperClient())
                {
                    byte[] downloadedFile = w.DownloadData(url);
                    return downloadedFile;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message, ConsoleColor.DarkRed);
                return null;
            }
        }
        private static void WriteFile(byte[] bytes, string fullpath)
        {
            CreateEmptyFile(fullpath);
            File.WriteAllBytes(fullpath, bytes);
        }
        private static void CreateEmptyFile(string fullpath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(fullpath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullpath));
            }
            if (!File.Exists(fullpath))
            {
                StreamWriter sw = new StreamWriter(fullpath, false);
                sw.Write(' ');
                sw.Close();
            }
        }
        private static void DecompressZip(string zipFullPath)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipFullPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string path = Path.Combine(FConfig.TempPath, VersionCheck.NameWithVersion, entry.FullName);
                    CreateEmptyFile(path);
                    entry.ExtractToFile(path, true);
                }
            }
        }

        public static void DownloadNewVersion()
        {
            Log.WriteLine(string.Format("starting to download {0} please wait...", VersionCheck.NameWithVersion), ConsoleColor.Green);
            byte[] downloaded = GetFile(VersionCheck.RemoteVersion);
            Log.WriteLine("download finished...", ConsoleColor.Green);
            WriteFile(downloaded, FConfig.TempRarFile);
            Log.WriteLine("decompressing now...", ConsoleColor.Green);
            DecompressZip(FConfig.TempRarFile);
            Log.WriteLine("files changing now...", ConsoleColor.Green);
            ChangeWithOldFiles(CreateUpdaterBatch());
        }

        /// <summary>
        /// returns fileupdater.bat full path
        /// </summary>
        /// <returns></returns>
        private static string CreateUpdaterBatch()
        {
            string path = Path.Combine(FConfig.TempPath, "FileUpdater.bat");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@echo off");
            sb.AppendLine("ECHO ### FILES CHANGING ###");
            sb.AppendLine(string.Format("ECHO ### {0}.exe  ###", VersionCheck.NameWithVersion));
            sb.AppendLine("ECHO .");
            sb.AppendLine("taskkill /F /IM \"MSniper.exe\"");
            sb.AppendLine("timeout /t 2");
            sb.AppendLine(string.Format("xcopy /s/y/e/q/r \"{0}\\{1}\" \"{2}\"", FConfig.TempPath, VersionCheck.NameWithVersion, FConfig.StartupPath));
            sb.AppendLine(string.Format("del /S \"{0}\\{1}\"", FConfig.StartupPath, "registerProtocol.bat"));
            sb.AppendLine(string.Format("del /S \"{0}\\{1}\"", FConfig.StartupPath, "removeProtocol.bat"));
            sb.AppendLine(string.Format("del /S \"{0}\\{1}\"", FConfig.StartupPath, "resetSnipeList.bat"));
            sb.AppendLine(string.Format("del /S \"{0}\\{1}\"", FConfig.StartupPath, "Newtonsoft.Json.dll"));
            sb.AppendLine(string.Format("start \"\" \"{0}\"", Process.GetCurrentProcess().MainModule.FileName));
            sb.AppendLine("ECHO ### FINISHED ###");
            File.WriteAllText(path, sb.ToString());
            return path;
        }

        /// <summary>
        /// running batch
        /// </summary>
        /// <param name="BatchPath"></param>
        private static void ChangeWithOldFiles(string BatchPath)
        {
            ProcessStartInfo psi = new ProcessStartInfo(BatchPath);
            Process proc = new Process();
            proc.StartInfo = psi;
            proc.Start();
            proc.WaitForExit();
            Process.GetCurrentProcess().Kill();
        }

    }
}
