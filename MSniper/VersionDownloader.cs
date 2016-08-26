using System;
using System.Collections.Generic;
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
            File.WriteAllBytes(fullpath, bytes);
        }
        private static void DecompressRar(string zipFullPath)
        {
            ZipFile.ExtractToDirectory(zipFullPath, FConfig.TempPath + "\\test\\");
        }

        public static void GetNewVersion()
        {
            byte[] downloaded = GetFile(VersionCheck.RemoteVersion);
            WriteFile(downloaded, FConfig.TempRarFile);
            DecompressRar(FConfig.TempRarFile);
        }

    }
}
