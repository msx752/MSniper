using System;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MSniper
{
    public static class VersionCheck
    {
        private static Version RVersion = null;
        public static string RemoteVersion => RVersion.ToString().Substring(0, 5);
        public static string NameWithVersion => "MSniper.v" + RemoteVersion;

        public static bool IsLatest()
        {
            try
            {
                var regex = new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]");
                var match = regex.Match(DownloadServerVersion());

                if (!match.Success)
                    return false;

                var gitVersion = new Version($"{match.Groups[1]}.{match.Groups[2]}.{match.Groups[3]}");
                RVersion = gitVersion;
                if (gitVersion > Assembly.GetExecutingAssembly().GetName().Version)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Program.frm.Console.WriteLine(ex.Message, Color.Red);
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