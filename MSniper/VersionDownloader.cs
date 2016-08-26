using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSniper
{
    public static class VersionDownloader
    {
        public static byte[] GetFile(string fileVersion)
        {
            try
            {
                string url = string.Format(FConfig.FileLink, fileVersion.ToString().Substring(0, 5));
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
    }
}
