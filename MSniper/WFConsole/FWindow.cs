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
using System.Threading.Tasks;
using System.Windows.Forms;
using MSniper.Enums;
using MSniper.Extensions;
using MSniper.Settings;
using MSniper.Settings.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MSniper.WFConsole
{
    public partial class FWindow : Form
    {
        #region form

        public FWindow()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            FormClosed += FWindow_FormClosed;

            #region menustrip actions

            getFeedsToolStripMenuItem.Click += delegate (object sender, EventArgs e)
            {
                //if (RunningNormally && BotCount > 0)
                //{
                //LFeed = new LiveFeed.LiveFeed();
                //LFeed.ShowDialog();
                //LFeed.Dispose();
                //}
                //else
                //{
                //    MessageBox.Show("necrobot not found or uninitialized normally");
                //}
            };
            donateToolStripMenuItem.Click += delegate (object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIoUri}#donation");
            };
            mSniperLatestToolStripMenuItem.Click += delegate (object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubProjectUri}");
            };
            necroBotLatestToolStripMenuItem.Click += delegate (object sender, EventArgs e)
            {
                Process.Start("https://github.com/NoxxDev/NecroBot/releases/latest");
            };
            featuresToolStripMenuItem.Click += delegate (object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIoUri}#features");
            };
            configurationToolStripMenuItem.Click += delegate (object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIoUri}#configuration");
            };
            usageToolStripMenuItem.Click += delegate (object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIoUri}#usage");
            };
            askedQuestionsToolStripMenuItem.Click += delegate (object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIoUri}#frequently-asked-questions");
            };
            advantageToolStripMenuItem.Click += delegate (object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIoUri}#advantage");
            };
            fileInformationToolStripMenuItem.Click += delegate (object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIoUri}#file-information");
            };

            #endregion menustrip actions

            Console.InitializeFConsole();
        }

        private void FWindow_FormClosed(object sender, FormClosedEventArgs e) => Process.GetCurrentProcess().Kill();

        private void FWindow_Load(object sender, EventArgs e)
        {
            this.Main();
        }

        #endregion form

        public LiveFeed.LiveFeed LFeed { get; set; }

        public int BotCount { get; set; }

        public bool RunningNormally { get; set; }

        public Configs Config { get; set; }

        public Translation Culture { get; set; }

        public Process[] GetNecroBotProcesses()
        {
            var plist = Process.GetProcesses().Where(x => x.ProcessName.ToLower().StartsWith(Variables.BotExeName) && !x.ProcessName.ToLower().EndsWith(".vshost")).ToArray();
            BotCount = plist.Count();
            return plist;
        }

        public void CheckNecroBots(bool shutdownMSniper)
        {
            if (GetNecroBotProcesses().Any()) return;
            Console.WriteLine(string.Empty.PadRight(10)+Culture.GetTranslation(TranslationString.AnyNecroBotNotFound), Config.Error);
            Console.WriteLine(string.Empty.PadRight(5) + $" *{Culture.GetTranslation(TranslationString.RunBeforeMSniper)}*", Config.Error);
            Console.WriteLine("--------------------------------------------------------");
            if (shutdownMSniper)
                Shutdown();
        }

        public void Delay(int seconds, bool isShutdownMsg = false)
        {
            for (var i = seconds; i >= 0; i--)
            {
                var ts = TranslationString.ShutdownMsg;
                if (!isShutdownMsg)
                    ts = TranslationString.SubsequentProcessing;

                Console.UpdateLine(Console.Lines.Count() - 1,
                    Culture != null
                        ? Culture.GetTranslation(ts, i)
                        : $"Subsequent processing passing in {i} seconds or Close the Program");

                Thread.Sleep(1000);
            }
        }

        public void ExportReferences()
        {
            try
            {
                var path = Path.Combine(Application.StartupPath, "Newtonsoft.Json.dll");
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
            }
            catch (Exception)
            {

            }
        }

        public void LoadConfigurations()
        {
            Config = LoadConfigs();
            Culture = LoadCulture(Config);

            #region controls culture
            activeBotsToolStripMenuItem.Text = Culture.GetTranslation(TranslationString.ActiveBots);
            linksToolStripMenuItem.Text = Culture.GetTranslation(TranslationString.HowTo);
            configurationToolStripMenuItem.Text = Culture.GetTranslation(TranslationString.Configuration);
            usageToolStripMenuItem.Text = Culture.GetTranslation(TranslationString.Usage);
            askedQuestionsToolStripMenuItem.Text = Culture.GetTranslation(TranslationString.AskedQuestions);
            advantageToolStripMenuItem.Text = Culture.GetTranslation(TranslationString.Advantage);
            fileInformationToolStripMenuItem.Text = Culture.GetTranslation(TranslationString.FileInformation);
            featuresToolStripMenuItem.Text = Culture.GetTranslation(TranslationString.Features);
            projectToolStripMenuItem.Text = Culture.GetTranslation(TranslationString.ProjectsLink);
            getFeedsToolStripMenuItem.Text = Culture.GetTranslation(TranslationString.GetLiveFeed);
            donateToolStripMenuItem.Text = Culture.GetTranslation(TranslationString.Donate);

            #endregion

            ////ExportDefaultTranslation(); //needs for translation information
        }

        public void Helper(bool withParams)
        {
            Console.WriteLine("\n--------------------------------------------------------");
            Console.WriteLine(Culture.GetTranslation(TranslationString.Description,
                Variables.ProgramName, Variables.CurrentVersion, Variables.By));
            Console.WriteLine(Culture.GetTranslation(TranslationString.GitHubProject,
                Variables.GithubIoUri),
                Config.Warning);
            Console.Write(Culture.GetTranslation(TranslationString.CurrentVersion,
                Assembly.GetEntryAssembly().GetName().Version.ToString()),
                Config.Highlight);
            if (Protocol.IsRegistered() == false && withParams == false)
            {
                Console.WriteLine(" ");
                Console.WriteLine(Culture.GetTranslation(TranslationString.ProtocolNotFound,
                "registerProtocol.bat"),
                Config.Error);
                Shutdown();
            }

            if (VersionCheck.IsLatest())
            {
                Console.WriteLine($"\t* {Culture.GetTranslation(TranslationString.LatestVersion)} *", Config.Highlight);
            }
            else
            {
                Console.WriteLine(string.Format($"* {Culture.GetTranslation(TranslationString.NewVersion)}: {{0}} *", VersionCheck.RemoteVersion), Config.Success);

                var downloadlink = Variables.GithubProjectUri + "/releases/latest";
                Console.WriteLine(string.Format($"* {Culture.GetTranslation(TranslationString.DownloadLink)}:  {{0}} *", downloadlink), Config.Warning);
                if (Config.DownloadNewVersion && withParams == false)
                {
                    Console.WriteLine(Culture.GetTranslation(TranslationString.AutoDownloadMsg), Config.Notification);
                    Console.Write($"{Culture.GetTranslation(TranslationString.Warning)}:", Config.Error);
                    Console.WriteLine(Culture.GetTranslation(TranslationString.WarningShutdownProcess), Config.Highlight);
                    var c = Console.ReadKey();
                    if (c == 'd' || c == 'D')
                    {
                        Downloader.DownloadNewVersion();
                    }
                    Shutdown();
                }
            }
            Console.WriteLine(Culture.GetTranslation(TranslationString.IntegrateMsg,
                Variables.ProgramName, Variables.MinRequireVersion), Config.Notification);
            Console.WriteLine("--------------------------------------------------------");
        }

        public void ShowActiveBots()
        {
            if (Config.ShowActiveBots)
            {
                Task.Run(() =>
                {
                    do
                    {
                        try
                        {
                            var plist = GetNecroBotProcesses();
                            activeBotsToolStripMenuItem.Visible = plist.Length != 0;
                            IsActiveBotsAlive();
                            foreach (var item in plist)
                            {
                                var username = item.GetWindowTitle();
                                if (string.IsNullOrEmpty(username))
                                    continue;

                                var btn = new ToolStripMenuItem(username) {Tag = item.MainWindowHandle};
                                btn.Click += delegate (object sender, EventArgs e)
                                {
                                    var id = int.Parse(btn.Tag.ToString());
                                    Dlls.BringToFront(new IntPtr(id));
                                };
                                var isExists = ActiveBotsContains(btn.Tag as IntPtr?);
                                if (isExists == -1)
                                {
                                    activeBotsToolStripMenuItem.DropDownItems.Add(btn);
                                }
                                else
                                {
                                    if (btn.Text == activeBotsToolStripMenuItem.DropDownItems[isExists].Text) continue;
                                    activeBotsToolStripMenuItem.DropDownItems.RemoveAt(isExists);
                                    activeBotsToolStripMenuItem.DropDownItems.Add(btn);
                                }
                            }
                        }
                        catch { }
                        Thread.Sleep(5000);
                    } while (true);
                });
            }
            else
            {
                activeBotsToolStripMenuItem.Visible = false;
            }
        }

        private void IsActiveBotsAlive()
        {
            for (var i = 0; i < activeBotsToolStripMenuItem.DropDownItems.Count; i++)
            {
                var prc = GetNecroBotProcesses()
               .FirstOrDefault(p => p.MainWindowHandle.ToString() == activeBotsToolStripMenuItem.DropDownItems[i].Tag.ToString());
                if (prc == null)
                {
                    activeBotsToolStripMenuItem.DropDownItems.RemoveAt(i);
                }
            }
        }

        private int ActiveBotsContains(IntPtr? intptr)
        {
            for (var i = 0; i < activeBotsToolStripMenuItem.DropDownItems.Count; i++)
            {
                if (intptr != null && activeBotsToolStripMenuItem.DropDownItems[i].Tag.ToString() == intptr.Value.ToString())
                    return i;
            }
            return -1;
        }

        public void Main()
        {
            Task.Run(() =>
            {
                Console.Clear();
                ExportReferences();
                LoadConfigurations();
                ShowActiveBots();
                Console.Title = Culture.GetTranslation(TranslationString.Title,
                    Variables.ProgramName, Variables.CurrentVersion, Variables.By
                    );
                Helper(Console.Arguments.Length != 0);
                CheckNecroBots(Console.Arguments.Length != 1);
                //args = new string[] { "msniper://Ivysaur/-33.890835,151.223859" };//for debug mode
                //args = new string[] { "-registerp" };//for debug mode
                if (Console.Arguments.Length == 1)
                {
                    RunningNormally = false;
                    switch (Console.Arguments.First())
                    {
                        case "-register":
                            Runas(Variables.ExecutablePath, "-registerp");
                            break;

                        case "-registerp":
                            Protocol.Register();
                            Console.WriteLine(Culture.GetTranslation(TranslationString.ProtocolRegistered), Config.Success);
                            break;

                        case "-remove":
                            Runas(Variables.ExecutablePath, "-removep");
                            break;

                        case "-removep":
                            Protocol.Delete();
                            Console.WriteLine(Culture.GetTranslation(TranslationString.ProtocolDeleted), Config.Success);
                            break;

                        case "-resetallsnipelist":
                            RemoveAllSnipeMsjson();
                            break;

                        default:
                            var re0 = "(pokesniper2://|msniper://)"; //protocol
                            var re1 = "((?:\\w+))";//pokemon name
                            var re2 = "(\\/)";//separator
                            var re3 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lat
                            var re4 = "(,)";//separator
                            var re5 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lon

                            var r = new Regex(re0 + re1 + re2 + re3 + re4 + re5, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            var m = r.Match(Console.Arguments.First());
                            if (m.Success)
                            {
                                Snipe(m.Groups[2].ToString(), m.Groups[4].ToString(), m.Groups[6].ToString());
                            }
                            else
                            {
                                Console.WriteLine(Culture.GetTranslation(TranslationString.UnknownLinkFormat), Config.Error);
                            }
                            break;
                    }
                }
                else if (Console.Arguments.Length == 0)
                {
                    RunningNormally = true;
                    Console.WriteLine(Culture.GetTranslation(TranslationString.CustomPasteDesc));
                    Console.WriteLine(Culture.GetTranslation(TranslationString.CustomPasteFormat));
                    Console.WriteLine("--------------------------------------------------------");
                    do
                    {
                        Console.WriteLine(Culture.GetTranslation(TranslationString.WaitingDataMsg), Config.Highlight);
                        var snipping = Console.ReadLine();
                        CheckNecroBots(true);
                        //snipping = "dragonite 37.766627 , -122.403677";//for debug mode (spaces are ignored)
                        if (snipping.ToLower() == "e")
                            break;
                        var re1 = "((?:\\w+))";//pokemon name
                        var re2 = "( )";//separator
                        var re3 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lat
                        var re4 = "(\\s*,\\s*)";//separator
                        var re5 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lon
                        var r = new Regex(re1 + re2 + re3 + re4 + re5, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                        var error = true;
                        foreach (Match m in r.Matches(snipping))
                        {
                            if (!m.Success) continue;
                            var pokemonN = m.Groups[1].ToString();
                            error = false;
                            var prkmnm = PokemonId.Abra;
                            var verified = Enum.TryParse<PokemonId>(pokemonN, true, out prkmnm);
                            if (verified)
                                Snipe(pokemonN, m.Groups[3].ToString(), m.Groups[5].ToString());
                            else
                                Console.WriteLine(Culture.GetTranslation(TranslationString.WrongPokemonName, pokemonN), Config.Error);
                        }
                        if (error)
                            Console.WriteLine(Culture.GetTranslation(TranslationString.CustomPasteWrongFormat), Config.Error);
                    }
                    while (true);
                }
                Shutdown();
            });
        }

        public void RemoveAllSnipeMsjson()
        {
            var plist = GetNecroBotProcesses();
            foreach (var item in plist)
            {
                var pathRemote = GetSnipeMsPath(Path.GetDirectoryName(item.MainModule.FileName));
                if (File.Exists(pathRemote))
                {
                    FileDelete(pathRemote);
                    var val = item.MainWindowTitle.Split('-').First().Split(' ')[2];
                    Console.WriteLine(Culture.GetTranslation(TranslationString.RemoveAllSnipe, val), Config.Success);
                }
            }
            Console.WriteLine(Culture.GetTranslation(TranslationString.RemoveAllSnipeFinished, plist.Count()), Config.Success);
        }

        public void Runas(string executablepath, string parameters, bool afterKillSelf = true)
        {
            var psi = new ProcessStartInfo(Application.ExecutablePath)
            {
                Verb = "runas",
                Arguments = parameters
            };
            Process.Start(psi);
            if (afterKillSelf)
                Process.GetCurrentProcess().Kill();
        }

        public void Shutdown()
        {
            Shutdown(Config.CloseDelaySec);
        }

        public void Shutdown(int seconds)
        {
            Console.State = ConsoleState.Closing;
            Delay(seconds, true);
            Process.GetCurrentProcess().Kill();
        }

        public void Snipe(string pokemonName, string latt, string lonn)
        {
            try
            {
                var lat = double.Parse(latt, CultureInfo.InvariantCulture);
                var lon = double.Parse(lonn, CultureInfo.InvariantCulture);
                var pList = GetNecroBotProcesses();
                foreach (var t in pList)
                {
                    var username = t.GetWindowTitle();
                    if (string.IsNullOrEmpty(username))
                        continue;

                    if (!IsBotUpperThan094(t.MainModule.FileVersionInfo))
                    {
                        Console.WriteLine(Culture.GetTranslation(TranslationString.IncompatibleVersionMsg, username), Config.Error);
                        continue;
                    }
                    var pathRemote = GetSnipeMsPath(Path.GetDirectoryName(t.MainModule.FileName));
                    var mSniperLocation = ReadSnipeMs(pathRemote);
                    var newPokemon = new MSniperInfo()
                    {
                        Latitude = lat,
                        Longitude = lon,
                        Id = pokemonName
                    };
                    if (mSniperLocation.FindIndex(p => p.Id == newPokemon.Id && p.Latitude == newPokemon.Latitude && p.Longitude == newPokemon.Longitude) == -1)
                    {
                        mSniperLocation.Add(newPokemon);
                        if (WriteSnipeMs(mSniperLocation, pathRemote))
                        {
                            Console.WriteLine(Culture.GetTranslation(TranslationString.SendingPokemonToNecroBot,
                                newPokemon.Id.ToLower(),
                                newPokemon.Latitude,
                                newPokemon.Longitude,
                                username), Config.Success);
                        }
                    }
                    else
                    {
                        Console.WriteLine(Culture.GetTranslation(TranslationString.AlreadySnipped, newPokemon), Config.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, Config.Error);
            }
        }

        private static void ExportDefaultTranslation()
        {
            // this method only using for information \temp\languages\translation.en.json
            var culture = new Translation();
            culture.Save("en");
            Process.GetCurrentProcess().Kill();
        }

        private static string GetSnipeMsPath(string necroBotExePath) => Path.Combine(necroBotExePath, Variables.SnipeFileName);

        private static Configs LoadConfigs()
        {

            var settings = new Configs();
            if (File.Exists(Variables.SettingPath))
            {
                settings = Configs.Load();
            }
            else
            {
                Configs.SaveFiles(settings);
            }
            return settings;
        }

        private static List<MSniperInfo> ReadSnipeMs(string path)
        {
            if (File.Exists(path))
            {
                var jsn = "";
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

        private static bool WriteSnipeMs(List<MSniperInfo> mSniperLocation, string path)
        {
            do
            {
                try
                {
                    var sw = new StreamWriter(path, false, Encoding.UTF8);
                    sw.WriteLine(JsonConvert.SerializeObject(
                        mSniperLocation,
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

        private bool IsBotUpperThan094(FileVersionInfo fversion) => new Version(fversion.FileVersion) >= new Version(Variables.MinRequireVersion);

        private Translation LoadCulture(Configs settings) => Translation.Load(settings);
    }
}