using System;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MSniper
{
    public static class VersionCheck
    {
        private static Version _rVersion = null;
        public static string RemoteVersion => _rVersion.ToString();
        public static string NameWithVersion => "MSniper.v" + RemoteVersion;

        public static bool IsLatest()
        {
            try
            {
                var regex = new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]");
                var match = regex.Match(DownloadServerVersion());

                if (!match.Success)
                    return false;

                var gitVersion = new Version($"{match.Groups[1]}.{match.Groups[2]}.{match.Groups[3]}.{match.Groups[4]}");
                _rVersion = gitVersion;
                if (gitVersion > Assembly.GetExecutingAssembly().GetName().Version)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Program.frm.Console.WriteLine(ex.Message, Program.frm.Config.Error);
                return true; //better than just doing nothing when git server down
            }

            return true;
        }

        private static string DownloadServerVersion()
        {
           return Downloader.DownloadString(Variables.VersionUri);
        }
    }
}