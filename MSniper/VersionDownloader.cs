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
                    string path = Path.Combine(FConfig.StartupPath, entry.FullName);
                    CreateEmptyFile(path);
                    entry.ExtractToFile(path, true);
                }
            }
        }

        public static void GetNewVersion()
        {
            byte[] downloaded = GetFile(VersionCheck.RemoteVersion);
            WriteFile(downloaded, FConfig.TempRarFile);
            DecompressZip(FConfig.TempRarFile);
        }

        public static void CreateBatch()
        {
            string path = Path.Combine(FConfig.StartupPath, "FileUpdater.bat");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@echo off");
            sb.AppendLine("taskkill /F /IM \"" + new FileInfo(Process.GetCurrentProcess().MainModule.FileName).Name.Replace(".vshost", "")+"\"");
            sb.AppendLine("timeout /t 2");
            sb.AppendLine(string.Format("xcopy /s/y/e/q/r \"{0}\\{1}\" \"{2}\"", FConfig.TempPath, VersionCheck.NameWithVersion, FConfig.StartupPath));
            File.WriteAllText(path, sb.ToString());
        }
    }
}
