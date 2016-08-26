using System.IO;
using System.Windows.Forms;

namespace MSniper
{
    public static class Variables
    {
        public static string GithupProjectLink => "https://github.com/msx752/MSniper";
        public static string FileLink => GithupProjectLink + "/releases/download/{0}/MSniper.v{0}.zip";
        public static string TempRarFile => Path.Combine(TempPath, VersionCheck.NameWithVersion + ".zip");
        public static string VersionUri => "https://raw.githubusercontent.com/msx752/MSniper/master/MSniper/Properties/AssemblyInfo.cs";
        public static string BotEXEName => "necrobot";
        public static string Snipefilename => "SnipeMS.json";
        public static string MinRequireVersion => "0.9.5";
        public static string ExecutablePath => Application.ExecutablePath;
        public static string StartupPath => Path.GetDirectoryName(ExecutablePath);
        public static string TempPath => Path.Combine(Application.StartupPath, "temp");
    }
}