using MSniper.Settings.Localization;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MSniper
{
    public static class Downloader
    {
        public static byte[] DownloadData(string url)
        {
            using (var w = new MSniperClient())
            {
                w.Encoding = Encoding.Unicode;
                return w.DownloadData(url);
            }
        }

        public static void DownloadNewVersion()
        {
            Program.frm.Console.WriteLine(Program.frm.Culture.GetTranslation(TranslationString.DownloadingNewVersion, VersionCheck.NameWithVersion), Program.frm.Config.Success);
            byte[] downloaded = GetFile(VersionCheck.RemoteVersion);
            Program.frm.Console.WriteLine(Program.frm.Culture.GetTranslation(TranslationString.DownloadFinished), Program.frm.Config.Success);
            WriteFile(downloaded, Variables.TempRarFileUri);
            Program.frm.Console.WriteLine(Program.frm.Culture.GetTranslation(TranslationString.DecompressingNewFile), Program.frm.Config.Success);
            DecompressZip(Variables.TempRarFileUri);
            Program.frm.Console.WriteLine(Program.frm.Culture.GetTranslation(TranslationString.OldFilesChangingWithNews), Program.frm.Config.Success);
            ChangeWithOldFiles();
        }

        public static string DownloadString(string url)
        {
            using (var w = new MSniperClient())
            {
                return w.DownloadString(url);
            }
        }

        /// <summary>
        /// running batch 
        /// </summary>
        /// <param name="BatchPath">
        /// </param>
        private static void ChangeWithOldFiles()
        {
            //https://github.com/msx752/MSniper/blob/master/MSniper/MSniperUpdater.exe1?raw=true
            string url = $"{Variables.GithubProjectUri}/blob/master/MSniper/MSniperUpdater.exe1?raw=true";
            string updater = Path.Combine(Application.StartupPath, "MSniperUpdater.exe");
            byte[] updaterData = DownloadData(url);
            File.WriteAllBytes(updater, updaterData);
            Thread.Sleep(500);
            var psi = new ProcessStartInfo(updater, VersionCheck.NameWithVersion);
            var proc = new Process {StartInfo = psi};
            proc.Start();
            proc.WaitForExit();
            Process.GetCurrentProcess().Kill();
        }

        private static void CreateEmptyFile(string fullpath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(fullpath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullpath));
            }
            if (File.Exists(fullpath)) return;
            var sw = new StreamWriter(fullpath, false);
            sw.Write(' ');
            sw.Close();
        }

        ///// <summary>
        ///// returns fileupdater.bat full path 
        ///// </summary>
        ///// <returns>
        ///// </returns>
        //private static string CreateUpdaterBatch()
        //{
        //    var path = Path.Combine(Variables.TempPath, "fileUpdater.bat");
        //    var sb = new StringBuilder();
        //    sb.AppendLine("@echo off");
        //    sb.AppendLine("ECHO ### FILES CHANGING ###");
        //    sb.AppendLine($"ECHO ### {VersionCheck.NameWithVersion}.exe  ###");
        //    sb.AppendLine("ECHO .");
        //    sb.AppendLine("taskkill /F /IM \"MSniper.exe\"");
        //    sb.AppendLine("timeout /t 1");
        //    sb.AppendLine($"xcopy /s/y/e/q/r \"%cd%\\temp\\{VersionCheck.NameWithVersion}\" \"%cd%\"");
        //    sb.AppendLine("timeout /t 1");
        //    sb.AppendLine("del /S \"..\\registerProtocol.bat\"");
        //    sb.AppendLine("del /S \"..\\removeProtocol.bat\"");
        //    sb.AppendLine("del /S \"..\\resetSnipeList.bat\"");
        //    sb.AppendLine("del /S \"..\\Newtonsoft.Json.dll\"");
        //    sb.AppendLine("timeout /t 1");
        //    sb.AppendLine("start \"\" \"%cd%\\MSniper.exe\"");
        //    sb.AppendLine("ECHO ### FINISHED ###");
        //    File.WriteAllText(path, sb.ToString());
        //    return path;
        //}

        private static void DecompressZip(string zipFullPath)
        {
            using (var archive = ZipFile.OpenRead(zipFullPath))
            {
                foreach (var entry in archive.Entries)
                {
                    var path = Path.Combine(Variables.TempPath, VersionCheck.NameWithVersion, entry.FullName);
                    CreateEmptyFile(path);
                    entry.ExtractToFile(path, true);
                }
            }
        }

        private static byte[] GetFile(string fileVersion)
        {
            try
            {
                var url = string.Format(Variables.FileLink, fileVersion);
                var downloadedFile = DownloadData(url);
                return downloadedFile;
            }
            catch (Exception ex)
            {
                try
                {
                    return GetFile(VersionCheck.RemoteVersion.Substring(0, 5));
                }
                catch { }
                Program.frm.Console.WriteLine(ex.Message, Program.frm.Config.Error);
                return null;
            }
        }

        private static void WriteFile(byte[] bytes, string fullpath)
        {
            CreateEmptyFile(fullpath);
            File.WriteAllBytes(fullpath, bytes);
        }
    }
}