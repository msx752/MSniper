using MSniper;
using MSniper.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
        ShutdownMsg,
        LatestVersion,
        NewVersion,
        DownloadLink,
        AutoDownloadMsg,
        Warning,
        WarningShutdownProcess,
        IntegrateMsg,
        AnyNecroBotNotFound,
        RunBeforeMSniper,
        ProtocolRegistered,
        ProtocolDeleted,
        RemoveAllSnipe,
        RemoveAllSnipeFinished,
        UnknownLinkFormat,
        CustomPasteDesc,
        CustomPasteFormat,
        WaitingDataMsg,
        CustomPasteWrongFormat,
        IncompatibleVersionMsg,
        SendingPokemonToNecroBot,
        AlreadySnipped,
        DownloadingNewVersion,
        DownloadFinished,
        DecompressingNewFile,
        OldFilesChangingWithNews,
        SubsequentProcessing,
        WrongPokemonName,
        ActiveBots,
        HowTo,
        Configuration,
        Usage,
        AskedQuestions,
        Advantage,
        FileInformation,
        Features,
        ProjectsLink,
        GetLiveFeed,
        Donate,
        CanNotAccessProcess,
    }
    /// <summary>
    /// default language: english
    /// </summary>
    public class Translation : ITranslation
    {
        [JsonProperty("TranslationStrings",
              ItemTypeNameHandling = TypeNameHandling.Arrays,
              ItemConverterType = typeof(KeyValuePairConverter),
              ObjectCreationHandling = ObjectCreationHandling.Replace,
              DefaultValueHandling = DefaultValueHandling.Populate)]
        private readonly List<KeyValuePair<TranslationString, string>> _translationStrings = new List
            <KeyValuePair<TranslationString, string>>
        {
            new KeyValuePair<TranslationString, string>(TranslationString.Title, "[{0} v{1}]  -  by {2}"),
            new KeyValuePair<TranslationString, string>(TranslationString.Description, "{0} - v{1} Manual Pokemon Sniper - by {2}"),
            new KeyValuePair<TranslationString, string>(TranslationString.GitHubProject, "Github Project {0}"),
            new KeyValuePair<TranslationString, string>(TranslationString.CurrentVersion, "Current Version: {0}"),
            new KeyValuePair<TranslationString, string>(TranslationString.ProtocolNotFound, "Protocol not found - Please run once {0}"),
            new KeyValuePair<TranslationString, string>(TranslationString.ShutdownMsg, "Program is closing in {0} seconds"),
            new KeyValuePair<TranslationString, string>(TranslationString.LatestVersion, "Latest Version"),
            new KeyValuePair<TranslationString, string>(TranslationString.NewVersion, "NEW VERSION"),
            new KeyValuePair<TranslationString, string>(TranslationString.DownloadLink, "DOWNLOAD LINK"),
            new KeyValuePair<TranslationString, string>(TranslationString.AutoDownloadMsg, "PRESS 'D' TO AUTOMATIC DOWNLOAD NEW VERSION OR PRESS ANY KEY FOR EXIT.."),
            new KeyValuePair<TranslationString, string>(TranslationString.Warning, "WARNING"),
            new KeyValuePair<TranslationString, string>(TranslationString.WarningShutdownProcess, "All MSniper.exe will shutdown while downloading"),
            new KeyValuePair<TranslationString, string>(TranslationString.IntegrateMsg, "{0} integrated NecroBot v{1} or upper"),
            new KeyValuePair<TranslationString, string>(TranslationString.AnyNecroBotNotFound, "Any running NecroBot not found..."),
            new KeyValuePair<TranslationString, string>(TranslationString.RunBeforeMSniper, "Necrobot must be running before MSniper"),
            new KeyValuePair<TranslationString, string>(TranslationString.ProtocolRegistered, "Protocol Successfully REGISTERED:"),
            new KeyValuePair<TranslationString, string>(TranslationString.ProtocolDeleted, "Protocol Successfully DELETED:"),
            new KeyValuePair<TranslationString, string>(TranslationString.RemoveAllSnipe, "deleted for {0}"),
            new KeyValuePair<TranslationString, string>(TranslationString.RemoveAllSnipeFinished, "deleting finished process count:{0}.."),
            new KeyValuePair<TranslationString, string>(TranslationString.UnknownLinkFormat, "unknown link format"),
            new KeyValuePair<TranslationString, string>(TranslationString.CustomPasteDesc, "\t\tCUSTOM PASTE ACTIVE"),
            new KeyValuePair<TranslationString, string>(TranslationString.CustomPasteFormat, "format: PokemonName Latitude,Longitude"),
            new KeyValuePair<TranslationString, string>(TranslationString.WaitingDataMsg, "waiting data.."),
            new KeyValuePair<TranslationString, string>(TranslationString.CustomPasteWrongFormat, "wrong format retry or write 'E' for quit.."),
            new KeyValuePair<TranslationString, string>(TranslationString.IncompatibleVersionMsg, "Incompatible NecroBot version for {0}"),
            new KeyValuePair<TranslationString, string>(TranslationString.SendingPokemonToNecroBot, "Sending to {3}: {0} {1},{2}"),
            new KeyValuePair<TranslationString, string>(TranslationString.AlreadySnipped, "{0}\t\tAlready Snipped..."),
            new KeyValuePair<TranslationString, string>(TranslationString.DownloadingNewVersion, "starting to download {0} please wait..."),
            new KeyValuePair<TranslationString, string>(TranslationString.DownloadFinished, "download finished..."),
            new KeyValuePair<TranslationString, string>(TranslationString.DecompressingNewFile,"decompressing now..."),
            new KeyValuePair<TranslationString, string>(TranslationString.OldFilesChangingWithNews,"files changing now..."),
            new KeyValuePair<TranslationString, string>(TranslationString.SubsequentProcessing, "Subsequent processing passing in {0} seconds or Close the Program"),
            new KeyValuePair<TranslationString, string>(TranslationString.WrongPokemonName,"Pokemon '{0}' not defined"),
            new KeyValuePair<TranslationString, string>(TranslationString.ActiveBots,"Active NecroBots"),
            new KeyValuePair<TranslationString, string>(TranslationString.HowTo,"How To"),
            new KeyValuePair<TranslationString, string>(TranslationString.Configuration,"Configuration"),
            new KeyValuePair<TranslationString, string>(TranslationString.Usage,"Usage"),
            new KeyValuePair<TranslationString, string>(TranslationString.AskedQuestions,"Asked Questions"),
            new KeyValuePair<TranslationString, string>(TranslationString.Advantage,"Advantage"),
            new KeyValuePair<TranslationString, string>(TranslationString.FileInformation,"File Information"),
            new KeyValuePair<TranslationString, string>(TranslationString.Features,"Features"),
            new KeyValuePair<TranslationString, string>(TranslationString.ProjectsLink,"Projects Link"),
            new KeyValuePair<TranslationString, string>(TranslationString.GetLiveFeed,"Get Live Feed"),
            new KeyValuePair<TranslationString, string>(TranslationString.Donate,"Donate"),
            new KeyValuePair<TranslationString, string>(TranslationString.CanNotAccessProcess,"can not access to {0}.exe({1}), killing"),
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
            return translation != default(string)
                ? translation
                : $"Translation for {translationString} is missing";
        }

        public static Translation Load(IConfigs logicSettings)
        {
            return Load(logicSettings, new Translation());
        }

        public static Translation Load(IConfigs logicSettings, Translation translations)
        {
            try
            {
                var translationsLanguageCode = logicSettings.TranslationLanguageCode.Replace("-", "_");
                var input = "";
                if (Variables.SupportedLanguages.FindIndex(p => p.ToLower() == translationsLanguageCode) == -1)
                {
                    input = GetTranslationFromServer(logicSettings.TranslationLanguageCode);//developer mode
                    if (input == null)
                    {
                        Program.frm.Console.WriteLine($"[ {logicSettings.TranslationLanguageCode} ] language not found in program", Color.Red);
                        Thread.Sleep(1000);
                        Program.frm.Console.WriteLine($"now using default language [ {new Configs().TranslationLanguageCode} ]..", Color.Green);
                        translationsLanguageCode = new Configs().TranslationLanguageCode;
                        Program.frm.Delay(3);
                    }
                }
                if (string.IsNullOrEmpty(input))
                    input = Resources.ResourceManager.GetString($"translation_{translationsLanguageCode}");

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
                Program.frm.Console.WriteLine("");
                Program.frm.Console.WriteLine($"[ERROR] Issue loading translations: {ex.ToString()}", Color.Red);
                Program.frm.Delay(7);
            }

            return translations;
        }

        public static string GetTranslationFromServer(string languageCode)
        {
            try
            {
                return Downloader.DownloadString(string.Format(Variables.TranslationUri, languageCode));
            }
            catch
            {
                return null;
            }
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
