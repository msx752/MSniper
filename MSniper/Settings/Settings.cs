using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSniper.Settings
{
    public interface ISettings
    {
        int CloseDelaySec { get; }
        string TranslationLanguageCode { get; }
        bool DownloadNewVersion { get; set; }
    }

    public class Configs : ISettings
    {
        public int CloseDelaySec { get; set; } = 5;

        public string TranslationLanguageCode { get; set; } = "en";

        public bool DownloadNewVersion { get; set; } = true;

        public static Configs Load()
        {
            Configs _settings = new Configs();
            string configFile = Variables.SettingPath;
            if (File.Exists(configFile))
            {
                try
                {
                    //if the file exists, load the settings
                    string input = "";
                    int count = 0;
                    while (true)
                    {
                        try
                        {
                            input = File.ReadAllText(configFile);

                            break;
                        }
                        catch (Exception exception)
                        {
                            if (count > 10)
                            {
                                //sometimes we have to wait close to config.json for access
                                Log.WriteLine("configFile: " + exception.Message, ConsoleColor.Red);
                            }
                            count++;
                            Thread.Sleep(1000);
                        }
                    };

                    var jsonSettings = new JsonSerializerSettings();
                    jsonSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                    jsonSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;

                    try
                    {
                        _settings = JsonConvert.DeserializeObject<Configs>(input, jsonSettings);
                        SaveFiles(_settings);
                    }
                    catch (JsonSerializationException exception)
                    {
                        Log.WriteLine("Settings.json WRONG FORMAT: " + exception.Message, ConsoleColor.Red);
                        Program.Delay(30);
                    }
                }
                catch (JsonReaderException exception)
                {
                    Log.WriteLine("JSON Exception: " + exception.Message, ConsoleColor.Red);
                    return _settings;
                }
            }

            return _settings;
        }

        public static void SaveFiles(Configs settings)
        {
            settings.Save(Variables.SettingPath);
        }

        public void Save(string fullPath)
        {
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
