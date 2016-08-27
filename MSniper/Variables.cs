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
        public static string GithupRawUri => $"https://raw.githubusercontent.com/{By}/{ProgramName}";
        public static string FileLink => GithupProjectLink + $"/releases/download/{{0}}/{ProgramName}.v{{0}}.zip";
        public static string TempRarFileUri => Path.Combine(TempPath, VersionCheck.NameWithVersion + ".zip");
        public static string VersionUri => $"{GithupRawUri}/{By}/{ProgramName}/master/{ProgramName}/Properties/AssemblyInfo.cs";
        public static string TranslationUri => $"{GithupRawUri}/master/{ProgramName}/Settings/Localization/Languages/translation.{{0}}.json";
        public static string BotEXEName => "necrobot";
        public static string SnipeFileName => "SnipeMS.json";
        public static string SettingsFileName => "Settings.json";
        public static string MinRequireVersion => "0.9.5";
        public static string ExecutablePath => Application.ExecutablePath;
        public static string StartupPath => Path.GetDirectoryName(ExecutablePath);
        public static string TempPath => Path.Combine(Application.StartupPath, "temp");
        public static string SettingPath => Path.Combine(Application.StartupPath, SettingsFileName);
        public static string TranslationsPath => Path.Combine(Variables.TempPath, "Languages");

        public static List<string> SupportedLanguages => new List<string>()
        {
            "en","tr"
        };
    }
}