
using MSniper.Settings;
using MSniper.Settings.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace MSniper
{
    internal class Program
    {
        public static Translation culture { get; set; }
        public static Configs config { get; set; }

        private static void Helper(bool withParams)
        {
            Log.WriteLine("");
            Log.WriteLine(culture.GetTranslation(TranslationString.Description,
                Variables.ProgramName, Variables.CurrentVersion, Variables.By));
            Log.WriteLine(culture.GetTranslation(TranslationString.GitHubProject,
                Variables.GithupProjectLink),
                ConsoleColor.Yellow);
            Log.Write(culture.GetTranslation(TranslationString.CurrentVersion,
                Assembly.GetEntryAssembly().GetName().Version.ToString().Substring(0, 5)),
                ConsoleColor.White);
            if (Protocol.isRegistered() == false && withParams == false)
            {
                Log.WriteLine(" ");
                Log.WriteLine(culture.GetTranslation(TranslationString.ProtocolNotFound,
                "registerProtocol.bat"),
                ConsoleColor.Red);
                Shutdown();
            }
            if (VersionCheck.IsLatest())
            {
                Log.WriteLine($"\t* {culture.GetTranslation(TranslationString.LatestVersion)} *", ConsoleColor.White);
            }
            else
            {

                Log.WriteLine(string.Format($"\t* {culture.GetTranslation(TranslationString.NewVersion)}: {{0}} *", VersionCheck.RemoteVersion), ConsoleColor.Green);

                string downloadlink = Variables.GithupProjectLink + "/releases/latest";
                Log.WriteLine(string.Format($"* {culture.GetTranslation(TranslationString.DownloadLink)}:  {{0}} *", downloadlink), ConsoleColor.Yellow);
                Log.WriteLine(culture.GetTranslation(TranslationString.AutoDownloadMsg), ConsoleColor.DarkCyan);
                Log.Write($"{culture.GetTranslation(TranslationString.Warning)}:", ConsoleColor.Red);
                Log.WriteLine(culture.GetTranslation(TranslationString.WarningShutdownProcess), ConsoleColor.White);
                char c = Console.ReadKey().KeyChar;
                Console.SetCursorPosition(0, Console.CursorTop);
                if (c == 'd' || c == 'D')
                {
                    Downloader.DownloadNewVersion();
                }
                Shutdown();
            }
            Log.WriteLine(culture.GetTranslation(TranslationString.IntegrateMsg,
                Variables.ProgramName, Variables.MinRequireVersion), ConsoleColor.DarkCyan);
            Log.WriteLine("--------------------------------------------------------");
        }

        [STAThread]
        private static void Main(string[] args)
        {
            ExportReferences();
            //Translation culture = new Translation();
            //culture.Save("en");
            //return;
            Console.Title = culture.GetTranslation(TranslationString.Title,
                Variables.ProgramName, Variables.CurrentVersion, Variables.By
                );
            Console.Clear();
            Helper(args.Length == 0 ? false : true);
            CheckNecroBots(args.Length != 1);
            //args = new string[] { "msniper://Ivysaur/-33.890835,151.223859" };//for debug mode
            //args = new string[] { "-registerp" };//for debug mode
            if (args.Length == 1)
            {
                switch (args.First())
                {
                    case "-register":
                        Runas(Variables.ExecutablePath, "-registerp");
                        break;

                    case "-registerp":
                        Protocol.Register();
                        Log.WriteLine(culture.GetTranslation(TranslationString.ProtocolRegistered), ConsoleColor.Green);
                        break;

                    case "-remove":
                        Runas(Variables.ExecutablePath, "-removep");
                        break;

                    case "-removep":
                        Protocol.Delete();
                        Log.WriteLine(culture.GetTranslation(TranslationString.ProtocolDeleted), ConsoleColor.Green);
                        break;

                    case "-resetallsnipelist":
                        RemoveAllSnipeMSJSON();
                        break;

                    default:
                        string re0 = "(pokesniper2://|msniper://)";//protocol
                        string re1 = "((?:\\w+))";//pokemon name
                        string re2 = "(\\/)";//separator
                        string re3 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lat
                        string re4 = "(,)";//separator
                        string re5 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lon

                        Regex r = new Regex(re0 + re1 + re2 + re3 + re4 + re5, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        Match m = r.Match(args.First());
                        if (m.Success)
                        {
                            Snipe(m.Groups[2].ToString(), m.Groups[4].ToString(), m.Groups[6].ToString());
                        }
                        else
                        {
                            Log.WriteLine(culture.GetTranslation(TranslationString.UnknownLinkFormat), ConsoleColor.Red);
                        }
                        break;
                }
            }
            else if (args.Length == 0)
            {
                Log.WriteLine(culture.GetTranslation(TranslationString.CustomPasteDesc), ConsoleColor.DarkGray);
                Log.WriteLine(culture.GetTranslation(TranslationString.CustomPasteFormat), ConsoleColor.DarkGray);
                Log.WriteLine("--------------------------------------------------------");
                do
                {
                    Log.WriteLine(culture.GetTranslation(TranslationString.WaitingDataMsg), ConsoleColor.White);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    string snipping = Console.ReadLine();
                    CheckNecroBots(true);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    //snipping = "dragonite 37.766627 , -122.403677";//for debug mode (spaces are ignored)
                    if (snipping.ToLower() == "e")
                        break;
                    string re1 = "((?:\\w+))";//pokemon name
                    string re2 = "( )";//separator
                    string re3 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lat
                    string re4 = "(\\s*,\\s*)";//separator
                    string re5 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lon
                    Regex r = new Regex(re1 + re2 + re3 + re4 + re5, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match m = r.Match(snipping);
                    if (m.Success)
                    {
                        Snipe(m.Groups[1].ToString(), m.Groups[3].ToString(), m.Groups[5].ToString());
                    }
                    else
                    {
                        Log.WriteLine(culture.GetTranslation(TranslationString.CustomPasteWrongFormat), ConsoleColor.Red);
                    }
                }
                while (true);
            }
            Shutdown();
        }

        private static Configs LoadConfigs()
        {
            string strCulture = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var culture = CultureInfo.CreateSpecificCulture("en");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            Configs _settings = new Configs();
            if (File.Exists(Variables.SettingPath))
            {
                _settings = Configs.Load(Variables.SettingPath);
            }
            else
            {
                Configs.SaveFiles(_settings, Variables.SettingPath);
            }
            return _settings;
        }
        private static Translation LoadCulture(Configs _settings)
        {
            return Translation.Load(_settings);
        }
        private static void CheckNecroBots(bool shutdownMSniper)
        {
            if (Process.GetProcessesByName(Variables.BotEXEName).Count() == 0)
            {
                Log.WriteLine(culture.GetTranslation(TranslationString.AnyNecroBotNotFound), ConsoleColor.Red);
                Log.WriteLine($" *{culture.GetTranslation(TranslationString.RunBeforeMSniper)}*", ConsoleColor.Red);
                if (shutdownMSniper)
                    Shutdown();
            }
        }

        private static void RemoveAllSnipeMSJSON()
        {
            Process[] plist = Process.GetProcessesByName(Variables.BotEXEName);
            foreach (var item in plist)
            {
                string pathRemote = GetSnipeMSLocation(Path.GetDirectoryName(item.MainModule.FileName));
                if (File.Exists(pathRemote))
                {
                    FileDelete(pathRemote);
                    string val = item.MainWindowTitle.Split('-').First().Split(' ')[2];
                    Log.WriteLine(culture.GetTranslation(TranslationString.RemoveAllSnipe, val), ConsoleColor.Green);
                }
            }

            Log.WriteLine(culture.GetTranslation(TranslationString.RemoveAllSnipeFinished, plist.Count()), ConsoleColor.Green);
        }

        private static void Runas(string executablepath, string parameters, bool afterKillSelf = true)
        {
            ProcessStartInfo psi = new ProcessStartInfo(Application.ExecutablePath);
            psi.Verb = "runas";
            psi.Arguments = parameters;
            Process.Start(psi);
            if (afterKillSelf)
                Process.GetCurrentProcess().Kill();
        }

        private static void FileDelete(string path)
        {
            do
            {
                try
                {
                    File.Delete(path);
                    break;
                }
                catch { }
            } while (true);//waiting access
        }
        public static void Shutdown()
        {
            Shutdown(config.CloseDelaySec);
        }
        public static void Shutdown(int seconds)
        {
            for (int i = seconds; i >= 0; i--)
            {
                Log.WriteLine(culture.GetTranslation(TranslationString.ShutdownMsg,
                   i));
                setConsoleCursor(Log.lastLineCount, Console.CursorTop - 1);
                Thread.Sleep(1000);
                setDefaultConsoleCursor();
            }
            Process.GetCurrentProcess().Kill();
        }

        private static void setConsoleCursor(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        private static void setDefaultConsoleCursor()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        private static bool isBotUpperThan094(FileVersionInfo fversion)
        {
            if (new Version(fversion.FileVersion) >= new Version(Variables.MinRequireVersion))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void Snipe(string pokemonName, string latt, string lonn)
        {
            try
            {
                double lat = double.Parse(latt, CultureInfo.InvariantCulture);
                double lon = double.Parse(lonn, CultureInfo.InvariantCulture);
                foreach (var item in Process.GetProcessesByName(Variables.BotEXEName))
                {
                    string username = item.MainWindowTitle.Split('-').First().Split(' ')[2];
                    if (!isBotUpperThan094(item.MainModule.FileVersionInfo))
                    {
                        Log.WriteLine(culture.GetTranslation(TranslationString.IncompatibleVersionMsg, username), ConsoleColor.Red);
                        continue;
                    }
                    string pathRemote = GetSnipeMSLocation(Path.GetDirectoryName(item.MainModule.FileName));
                    List<MSniperInfo> MSniperLocation = ReadSnipeMS(pathRemote);
                    MSniperInfo newPokemon = new MSniperInfo()
                    {
                        Latitude = lat,
                        Longitude = lon,
                        Id = pokemonName
                    };
                    if (MSniperLocation.FindIndex(p => p.Id == newPokemon.Id && p.Latitude == newPokemon.Latitude && p.Longitude == newPokemon.Longitude) == -1)
                    {
                        MSniperLocation.Add(newPokemon);
                        if (WriteSnipeMS(MSniperLocation, newPokemon, pathRemote))
                        {
                            Log.WriteLine(culture.GetTranslation(TranslationString.SendingPokemonToNecroBot,
                                newPokemon.Id.ToLower(),
                                newPokemon.Latitude,
                                newPokemon.Longitude,
                                username), ConsoleColor.Green);
                        }
                    }
                    else
                    {
                        Log.WriteLine(culture.GetTranslation(TranslationString.AlreadySnipped, newPokemon), ConsoleColor.DarkRed);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message, ConsoleColor.DarkRed);
            }
        }

        private static void ExportReferences()
        {
            string path = Path.Combine(Application.StartupPath, "Newtonsoft.Json.dll");
            if (!File.Exists(path))
                File.WriteAllBytes(path, Properties.Resources.Newtonsoft_Json);
            path = Path.Combine(Application.StartupPath, "registerProtocol.bat");
            if (!File.Exists(path))
                File.WriteAllText(path, Properties.Resources.registerProtocol);
            path = Path.Combine(Application.StartupPath, "removeProtocol.bat");
            if (!File.Exists(path))
                File.WriteAllText(path, Properties.Resources.removeProtocol);
            path = Path.Combine(Application.StartupPath, "resetSnipeList.bat");
            if (!File.Exists(path))
                File.WriteAllText(path, Properties.Resources.resetSnipeList);

            //if (Directory.Exists(Variables.TempPath))//deleting temp
            //    Directory.Delete(Variables.TempPath, true);

            config = LoadConfigs();
            culture = LoadCulture(config);
        }

        private static string GetSnipeMSLocation(string NecroBotEXEPath)
        {
            return Path.Combine(NecroBotEXEPath, Variables.Snipefilename);
        }

        private static List<MSniperInfo> ReadSnipeMS(string path)
        {
            if (File.Exists(path))
            {
                string jsn = "";
                do
                {
                    try
                    {
                        jsn = File.ReadAllText(path, Encoding.UTF8);
                        break;
                    }
                    catch { Thread.Sleep(200); }
                }
                while (true);//waiting access
                return JsonConvert.DeserializeObject<List<MSniperInfo>>(jsn);
            }
            else
            {
                return new List<MSniperInfo>();
            }
        }

        private static bool WriteSnipeMS(List<MSniperInfo> _MSniperLocation, MSniperInfo _newpokemon, string path)
        {
            do
            {
                try
                {
                    StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8);
                    sw.WriteLine(JsonConvert.SerializeObject(
                        _MSniperLocation,
                        Formatting.Indented,
                        new StringEnumConverter { CamelCaseText = true }));
                    sw.Close();
                    return true;
                    break;
                }
                catch { Thread.Sleep(200); }
            }
            while (true);//waiting access
        }
    }
}