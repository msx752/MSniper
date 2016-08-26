using MSniper;
using MSniper.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSniper.Settings.Localization
{
    //code revised and getting from https://github.com/NoxxDev/NecroBot/blob/master/PoGo.NecroBot.Logic/Common/Translations.cs

    public interface ITranslation
    {
        string GetTranslation(TranslationString translationString, params object[] data);
        string GetTranslation(TranslationString translationString);
    }

    public enum TranslationString
    {
        Title,
        Description,
        GitHubProject,
        CurrentVersion,
        ProtocolNotFound,
        ShutdownMessage,
        LatestVersion,
        NewVersion,
        DownloadLink,
        AutoDownloadMessage,
        Warning,
        WarningShutdownProcess,
        IntegrateMessage,
    }
    /// <summary>
    /// default language: english
    /// </summary>
    public class Translation : ITranslation
    {
        public static List<string> SupportedLanguages => new List<string>()
        {
            "en"
        };

        [JsonProperty("TranslationStrings",
              ItemTypeNameHandling = TypeNameHandling.Arrays,
              ItemConverterType = typeof(KeyValuePairConverter),
              ObjectCreationHandling = ObjectCreationHandling.Replace,
              DefaultValueHandling = DefaultValueHandling.Populate)]
        private readonly List<KeyValuePair<TranslationString, string>> _translationStrings = new List
            <KeyValuePair<TranslationString, string>>
        {
            new KeyValuePair<TranslationString, string>(TranslationString.Title, "[{0} v{1}]  -  by {2}"),
            new KeyValuePair<TranslationString, string>(TranslationString.Description, "{0} - {1} Manual Pokemon Sniper - by {2}"),
            new KeyValuePair<TranslationString, string>(TranslationString.GitHubProject, "GitHub Project {0}"),
            new KeyValuePair<TranslationString, string>(TranslationString.CurrentVersion, "Current Version: {0}"),
            new KeyValuePair<TranslationString, string>(TranslationString.ProtocolNotFound, "Protocol not found - Please run once {0}"),
            new KeyValuePair<TranslationString, string>(TranslationString.ShutdownMessage, "Program is closing in {0} seconds"),
            new KeyValuePair<TranslationString, string>(TranslationString.LatestVersion, "Latest Version"),
            new KeyValuePair<TranslationString, string>(TranslationString.NewVersion, "NEW VERSION"),
            new KeyValuePair<TranslationString, string>(TranslationString.DownloadLink, "DOWNLOAD LINK"),
            new KeyValuePair<TranslationString, string>(TranslationString.AutoDownloadMessage, "PRESS 'D' TO AUTOMATIC DOWNLOAD NEW VERSION OR PRESS ANY KEY FOR EXIT.."),
            new KeyValuePair<TranslationString, string>(TranslationString.Warning, "WARNING"),
            new KeyValuePair<TranslationString, string>(TranslationString.WarningShutdownProcess, "All MSniper.exe will shutdown while downloading"),
            new KeyValuePair<TranslationString, string>(TranslationString.IntegrateMessage, "{0} integrated NecroBot v{1} or upper"),
        };

        public string GetTranslation(TranslationString translationString, params object[] data)
        {
            var translation = _translationStrings.FirstOrDefault(t => t.Key.Equals(translationString)).Value;
            return translation != default(string)
                ? string.Format(translation, data)
                : $"Translation for {translationString} is missing";
        }

        public string GetTranslation(TranslationString translationString)
        {
            var translation = _translationStrings.FirstOrDefault(t => t.Key.Equals(translationString)).Value;
            return translation != default(string) ? translation : $"Translation for {translationString} is missing";
        }

        public static Translation Load(ISettings logicSettings)
        {
            return Load(logicSettings, new Translation());
        }

        public static Translation Load(ISettings logicSettings, Translation translations)
        {
            var translationsLanguageCode = logicSettings.LanguageCode.Replace("-", "_");

            if (SupportedLanguages.FindIndex(p => p.ToLower() == translationsLanguageCode) == -1)
            {
                Log.WriteLine($"{logicSettings.LanguageCode} language not found in program", ConsoleColor.Red);
                Log.WriteLine($"now using default language..", ConsoleColor.Red);
                translationsLanguageCode = new Configs().LanguageCode;
            }

            string translationFile = $"translation_{translationsLanguageCode}";

            try
            {
                string input = Resources.ResourceManager.GetString(translationFile);
                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                jsonSettings.DefaultValueHandling = DefaultValueHandling.Populate;

                translations = JsonConvert.DeserializeObject<Translation>(input, jsonSettings);

                //TODO make json to fill default values as it won't do it now
                new Translation()._translationStrings.Where(
                    item => translations._translationStrings.All(a => a.Key != item.Key))
                    .ToList()
                    .ForEach(translations._translationStrings.Add);
            }
            catch (Exception ex)
            {
                Log.WriteLine($"[ERROR] Issue loading translations: {ex.ToString()}", ConsoleColor.DarkRed);
            }

            return translations;
        }

        public void Save(string languageCode)
        {
            string fullPath = $"{Variables.TranslationsPath}\\translation.{languageCode}.json";
            var output = JsonConvert.SerializeObject(this, Formatting.Indented,
                new StringEnumConverter { CamelCaseText = true });

            var folder = Path.GetDirectoryName(fullPath);
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.WriteAllText(fullPath, output);
        }

    }
}
