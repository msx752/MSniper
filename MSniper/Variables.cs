using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MSniper
{
    public static class Variables
    {
        public static string CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5);
        public static string By => "Msx752";
        public static string ProgramName => "MSniper";
        public static string GithupProjectLink => $"https://github.com/{By}/MSniper";
        public static string FileLink => GithupProjectLink + $"/releases/download/{{0}}/{ProgramName}.v{{0}}.zip";
        public static string TempRarFile => Path.Combine(TempPath, VersionCheck.NameWithVersion + ".zip");
        public static string VersionUri => $"https://raw.githubusercontent.com/{By}/{ProgramName}/master/{ProgramName}/Properties/AssemblyInfo.cs";
        public static string BotEXEName => "necrobot";
        public static string Snipefilename => "SnipeMS.json";
        public static string MinRequireVersion => "0.9.5";
        public static string ExecutablePath => Application.ExecutablePath;
        public static string StartupPath => Path.GetDirectoryName(ExecutablePath);
        public static string TempPath => Path.Combine(Application.StartupPath, "temp");
        public static string SettingPath => Path.Combine(Application.StartupPath, "Settings.json");
        public static string TranslationsPath => Path.Combine(Variables.TempPath, "Languages");

        public static List<string> SupportedLanguages => new List<string>()
        {
            "en","tr"
        };
    }
}