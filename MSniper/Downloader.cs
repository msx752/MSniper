using MSniper.Settings.Localization;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;

namespace MSniper
{
    public static class Downloader
    {
        public static byte[] DownloadData(string url)
        {
            using (MSniperClient w = new MSniperClient())
            {
                return w.DownloadData(url);
            }
        }

        public static void DownloadNewVersion()
        {
            Program.frm.Console.WriteLine(Program.frm.culture.GetTranslation(TranslationString.DownloadingNewVersion, VersionCheck.NameWithVersion), Program.frm.config.Success);
            byte[] downloaded = GetFile(VersionCheck.RemoteVersion);
            Program.frm.Console.WriteLine(Program.frm.culture.GetTranslation(TranslationString.DownloadFinished), Program.frm.config.Success);
            WriteFile(downloaded, Variables.TempRarFileUri);
            Program.frm.Console.WriteLine(Program.frm.culture.GetTranslation(TranslationString.DecompressingNewFile), Program.frm.config.Success);
            DecompressZip(Variables.TempRarFileUri);
            Program.frm.Console.WriteLine(Program.frm.culture.GetTranslation(TranslationString.OldFilesChangingWithNews), Program.frm.config.Success);
            ChangeWithOldFiles();
        }

        public static string DownloadString(string url)
        {
            using (MSniperClient w = new MSniperClient())
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
            string BatchPath = Path.Combine(Application.StartupPath, "msniperupdater.exe");
            ProcessStartInfo psi = new ProcessStartInfo(BatchPath, VersionCheck.NameWithVersion);
            Process proc = new Process();
            proc.StartInfo = psi;
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
            if (!File.Exists(fullpath))
            {
                StreamWriter sw = new StreamWriter(fullpath, false);
                sw.Write(' ');
                sw.Close();
            }
        }

        /// <summary>
        /// returns fileupdater.bat full path 
        /// </summary>
        /// <returns>
        /// </returns>
        private static string CreateUpdaterBatch()
        {
            string path = Path.Combine(Variables.TempPath, "fileUpdater.bat");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@echo off");
            sb.AppendLine("ECHO ### FILES CHANGING ###");
            sb.AppendLine(string.Format("ECHO ### {0}.exe  ###", VersionCheck.NameWithVersion));
            sb.AppendLine("ECHO .");
            sb.AppendLine("taskkill /F /IM \"MSniper.exe\"");
            sb.AppendLine("timeout /t 1");
            sb.AppendLine(string.Format("xcopy /s/y/e/q/r \"%cd%\\temp\\{0}\" \"%cd%\"", VersionCheck.NameWithVersion));
            sb.AppendLine("timeout /t 1");
            sb.AppendLine(string.Format("del /S \"..\\{0}\"", "registerProtocol.bat"));
            sb.AppendLine(string.Format("del /S \"..\\{0}\"", "removeProtocol.bat"));
            sb.AppendLine(string.Format("del /S \"..\\{0}\"", "resetSnipeList.bat"));
            sb.AppendLine(string.Format("del /S \"..\\{0}\"", "Newtonsoft.Json.dll"));
            sb.AppendLine("timeout /t 1");
            sb.AppendLine("start \"\" \"%cd%\\MSniper.exe\"");
            sb.AppendLine("ECHO ### FINISHED ###");
            File.WriteAllText(path, sb.ToString());
            return path;
        }

        private static void DecompressZip(string zipFullPath)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipFullPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string path = Path.Combine(Variables.TempPath, VersionCheck.NameWithVersion, entry.FullName);
                    CreateEmptyFile(path);
                    entry.ExtractToFile(path, true);
                }
            }
        }

        private static byte[] GetFile(string fileVersion)
        {
            try
            {
                string url = string.Format(Variables.FileLink, fileVersion);
                byte[] downloadedFile = DownloadData(url);
                return downloadedFile;
            }
            catch (Exception ex)
            {
                Program.frm.Console.WriteLine(ex.Message, Program.frm.config.Error);
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