using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSniper
{
    public static class VersionDownloader
    {
        public static byte[] GetFile(Version fileVersion)
        {
            try
            {
                using (MSniperClient w = new MSniperClient())
                {
                    w.Encoding = Encoding.UTF8;
                    byte[] downloadedFile = w.DownloadData(string.Format(FConfig.FileLink, fileVersion.ToString().Substring(0, 5)));
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
