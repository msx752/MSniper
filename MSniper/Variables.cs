using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MSniper
{
    public static class Variables
    {
        public static string BotExeName => "necrobot";
        public static string By => "Msx752";
        public static string CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string ExecutablePath => Application.ExecutablePath;
        public static string FileLink => $"{GithubProjectUri}/releases/download/{{0}}/{ProgramName}.v{{0}}.zip";
        public static string GithubProjectUri => $"https://github.com/{By}/{ProgramName}";
        public static string GithubRawUri => $"https://raw.githubusercontent.com/{By}/{ProgramName}";
        public static string GithubIoUri => $"https://github.com/{By}/{ProgramName}/";
        //
        public static string SnipeWebsite => $"http://msniper.com/";
        public static string MinRequireVersion => "1.0.0.0";
        public static string ProgramName => "MSniper";
        public static string SettingPath => Path.Combine(Application.StartupPath, SettingsFileName);
        public static string SettingsFileName => "Settings.json";
        public static string SnipeFileName => "SnipeMS.json";
        public static string StartupPath => Path.GetDirectoryName(ExecutablePath);

        public static string StrKillSwitchUri =
            "https://raw.githubusercontent.com/Necrobot-Private/NecroBot/master/KillSwitch.txt";
            //$"https://raw.githubusercontent.com/{By}/{ProgramName}/master/KillSwitch.txt";

        public static List<CultureInfo> SupportedLanguages => new List<CultureInfo>()
        {
            new CultureInfo("tr-TR"),
            new CultureInfo("zh-TW"),
            new CultureInfo("en-US"),
            new CultureInfo("es-ES"),
            new CultureInfo("it-IT")
        };

        public static string TempPath => Path.Combine(Application.StartupPath, "temp");
        public static string TempRarFileUri => Path.Combine(TempPath, VersionCheck.NameWithVersion + ".zip");
        public static string TranslationsPath => Path.Combine(Variables.TempPath, "Languages");
        public static string TranslationUri => $"{GithubRawUri}/master/{ProgramName}/Settings/Localization/Languages/translation.{{0}}.json";
        public static string VersionUri => $"{GithubRawUri}/master/{ProgramName}/Properties/AssemblyInfo.cs";
    }
}