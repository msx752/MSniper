using MSniper.Settings;
using MSniper.Settings.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSniper
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

            getFeedsToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                MessageBox.Show("Poke Feed - Coming Soon");
            };
            donateToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIOUri}#donation");
            };
            mSniperLatestToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIOUri}");
            };
            necroBotLatestToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                Process.Start("https://github.com/NoxxDev/NecroBot/releases/latest");
            };
            v102ToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIOUri}#v100--v102");
            };
            v103ToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIOUri}#v103");
            };
            v104ToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIOUri}#v104");
            };
            configurationToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIOUri}#configuration");
            };
            usageToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIOUri}#usage");
            };
            askedQuestionsToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIOUri}#frequently-asked-questions");
            };
            advantageToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIOUri}#advantage");
            };
            fileInformationToolStripMenuItem.Click += delegate (Object sender, EventArgs e)
            {
                Process.Start($"{Variables.GithubIOUri}#file-information");
            };

            #endregion menustrip actions

            Console.InitializeFConsole();
        }

        private void FWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void FWindow_Load(object sender, EventArgs e)
        {
            this.Main();
        }

        #endregion form

        public Configs config { get; set; }

        public Translation culture { get; set; }

        public Process[] GetNecroBotProcesses()
        {
            Process[] plist = Process.GetProcesses().Where(x => x.ProcessName.ToLower().StartsWith(Variables.BotEXEName)).ToArray();
            return plist;
        }
        public void CheckNecroBots(bool shutdownMSniper)
        {
            if (GetNecroBotProcesses().Count() == 0)
            {
                Console.WriteLine(culture.GetTranslation(TranslationString.AnyNecroBotNotFound), Color.Red);
                Console.WriteLine($" *{culture.GetTranslation(TranslationString.RunBeforeMSniper)}*", Color.Red);
                if (shutdownMSniper)
                    Shutdown();
            }
        }

        public void Delay(int seconds, bool isShutdownMsg = false)
        {
            for (int i = seconds; i >= 0; i--)
            {
                TranslationString ts = TranslationString.ShutdownMsg;
                if (!isShutdownMsg)
                    ts = TranslationString.SubsequentProcessing;

                if (culture != null)
                    Console.UpdateLine(Console.Lines.Count() - 1, culture.GetTranslation(ts, i));
                else
                    Console.UpdateLine(Console.Lines.Count() - 1, string.Format("Subsequent processing passing in {0} seconds or Close the Program", i));

                Thread.Sleep(1000);
            }
        }

        public void ExportReferences()
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

            if (Directory.Exists(Variables.TempPath))
            {
                if (config.DeleteTempFolder)//deleting temp folder
                {
                    try
                    {
                        Directory.Delete(Variables.TempPath, true);
                    }
                    catch
                    {
                        Directory.Delete(Variables.TempPath, true);
                    }
                }
            }
        }

        public void LoadConfigurations()
        {
            config = LoadConfigs();
            culture = LoadCulture(config);

            ////ExportDefaultTranslation(); //needs for translation information
        }

        public void Helper(bool withParams)
        {
            Console.WriteLine("");
            Console.WriteLine(culture.GetTranslation(TranslationString.Description,
                Variables.ProgramName, Variables.CurrentVersion, Variables.By));
            Console.WriteLine(culture.GetTranslation(TranslationString.GitHubProject,
                Variables.GithubProjectUri),
                Color.Yellow);
            Console.Write(culture.GetTranslation(TranslationString.CurrentVersion,
                Assembly.GetEntryAssembly().GetName().Version.ToString().Substring(0, 5)),
                Color.White);
            if (Protocol.isRegistered() == false && withParams == false)
            {
                Console.WriteLine(" ");
                Console.WriteLine(culture.GetTranslation(TranslationString.ProtocolNotFound,
                "registerProtocol.bat"),
                Color.Red);
                Shutdown();
            }

            if (VersionCheck.IsLatest())
            {
                Console.WriteLine($"\t* {culture.GetTranslation(TranslationString.LatestVersion)} *", Color.White);
            }
            else
            {
                Console.WriteLine(string.Format($"\t* {culture.GetTranslation(TranslationString.NewVersion)}: {{0}} *", VersionCheck.RemoteVersion), Color.Green);

                string downloadlink = Variables.GithubProjectUri + "/releases/latest";
                Console.WriteLine(string.Format($"* {culture.GetTranslation(TranslationString.DownloadLink)}:  {{0}} *", downloadlink), Color.Yellow);
                if (config.DownloadNewVersion && withParams == false)
                {
                    Console.WriteLine(culture.GetTranslation(TranslationString.AutoDownloadMsg), Color.DarkCyan);
                    Console.Write($"{culture.GetTranslation(TranslationString.Warning)}:", Color.Red);
                    Console.WriteLine(culture.GetTranslation(TranslationString.WarningShutdownProcess), Color.White);
                    char c = Console.ReadKey();
                    //Console.SetCursorPosition(0, Console.CursorTop);
                    if (c == 'd' || c == 'D')
                    {
                        Downloader.DownloadNewVersion();
                    }
                    Shutdown();
                }
            }
            Console.WriteLine(culture.GetTranslation(TranslationString.IntegrateMsg,
                Variables.ProgramName, Variables.MinRequireVersion), Color.DarkCyan);
            Console.WriteLine("--------------------------------------------------------");
        }

        public void ShowActiveBots()
        {
            if (config.ShowActiveBots)
            {
                Task.Run(() =>
                {
                    do
                    {
                        try
                        {
                            Process[] plist = GetNecroBotProcesses();
                            if (plist.Length == 0)
                                activeBotsToolStripMenuItem.DropDownItems.Clear();

                            foreach (Process item in plist)
                            {
                                string username = GetProcess(item).MainWindowTitle.Split('-').First().Split(' ')[2];
                                ToolStripMenuItem btn = new ToolStripMenuItem(username);
                                btn.Tag = item.MainWindowHandle;
                                btn.Click += delegate (Object sender, EventArgs e)
                                {
                                    int id = int.Parse(btn.Tag.ToString());
                                    Dlls.BringToFront(new IntPtr(id));
                                };
                                if (activeBotsToolStripMenuItem.DropDownItems.Count == 0)
                                    activeBotsToolStripMenuItem.DropDownItems.Add(btn);

                                for (int i = 0; i < activeBotsToolStripMenuItem.DropDownItems.Count; i++)
                                {
                                    var sn = activeBotsToolStripMenuItem.DropDownItems[i];
                                    if (sn != null && sn.Text != username)
                                    {
                                        activeBotsToolStripMenuItem.DropDownItems.Add(btn);
                                    }
                                    else
                                    {
                                        Process p2 = plist.Where(p => p.MainWindowTitle.IndexOf(activeBotsToolStripMenuItem.DropDownItems[i].Text) != -1).FirstOrDefault();
                                        if (p2 == null)
                                        {
                                            activeBotsToolStripMenuItem.DropDownItems.RemoveAt(i);
                                        }
                                    }
                                }
                            }
                        }
                        catch { }
                        Thread.Sleep(2000);
                    } while (true);
                });
            }
            else
            {
                activeBotsToolStripMenuItem.Visible = false;
            }
        }

        public void Main()
        {
            Task.Run(() =>
            {
                Console.Clear();
                ExportReferences();
                LoadConfigurations();
                ShowActiveBots();
                Console.Title = culture.GetTranslation(TranslationString.Title,
                    Variables.ProgramName, Variables.CurrentVersion, Variables.By
                    );
                Helper(Console.Arguments.Length == 0 ? false : true);
                CheckNecroBots(Console.Arguments.Length != 1);
                //args = new string[] { "msniper://Ivysaur/-33.890835,151.223859" };//for debug mode
                //args = new string[] { "-registerp" };//for debug mode
                if (Console.Arguments.Length == 1)
                {
                    switch (Console.Arguments.First())
                    {
                        case "-register":
                            Runas(Variables.ExecutablePath, "-registerp");
                            break;

                        case "-registerp":
                            Protocol.Register();
                            Console.WriteLine(culture.GetTranslation(TranslationString.ProtocolRegistered), Color.Green);
                            break;

                        case "-remove":
                            Runas(Variables.ExecutablePath, "-removep");
                            break;

                        case "-removep":
                            Protocol.Delete();
                            Console.WriteLine(culture.GetTranslation(TranslationString.ProtocolDeleted), Color.Green);
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
                            Match m = r.Match(Console.Arguments.First());
                            if (m.Success)
                            {
                                Snipe(m.Groups[2].ToString(), m.Groups[4].ToString(), m.Groups[6].ToString());
                            }
                            else
                            {
                                Console.WriteLine(culture.GetTranslation(TranslationString.UnknownLinkFormat), Color.Red);
                            }
                            break;
                    }
                }
                else if (Console.Arguments.Length == 0)
                {
                    Console.WriteLine(culture.GetTranslation(TranslationString.CustomPasteDesc));
                    Console.WriteLine(culture.GetTranslation(TranslationString.CustomPasteFormat));
                    Console.WriteLine("--------------------------------------------------------");
                    do
                    {
                        Console.WriteLine(culture.GetTranslation(TranslationString.WaitingDataMsg), Color.White);
                        string snipping = Console.ReadLine();
                        CheckNecroBots(true);
                        //snipping = "dragonite 37.766627 , -122.403677";//for debug mode (spaces are ignored)
                        if (snipping.ToLower() == "e")
                            break;
                        string re1 = "((?:\\w+))";//pokemon name
                        string re2 = "( )";//separator
                        string re3 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lat
                        string re4 = "(\\s*,\\s*)";//separator
                        string re5 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lon
                        Regex r = new Regex(re1 + re2 + re3 + re4 + re5, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                        bool error = true;
                        foreach (Match m in r.Matches(snipping))
                        {
                            if (m.Success)
                            {
                                string pokemonN = m.Groups[1].ToString();
                                error = false;
                                PokemonId prkmnm = PokemonId.Abra;
                                bool verified = Enum.TryParse<PokemonId>(pokemonN, true, out prkmnm);
                                if (verified)
                                    Snipe(pokemonN, m.Groups[3].ToString(), m.Groups[5].ToString());
                                else
                                    Console.WriteLine(culture.GetTranslation(TranslationString.WrongPokemonName, pokemonN), Color.Red);
                            }
                        }
                        if (error)
                            Console.WriteLine(culture.GetTranslation(TranslationString.CustomPasteWrongFormat), Color.Red);
                    }
                    while (true);
                }
                Shutdown();
            });
        }

        public void RemoveAllSnipeMSJSON()
        {
            Process[] plist = GetNecroBotProcesses();
            foreach (var item in plist)
            {
                string pathRemote = GetSnipeMSPath(Path.GetDirectoryName(item.MainModule.FileName));
                if (File.Exists(pathRemote))
                {
                    FileDelete(pathRemote);
                    string val = item.MainWindowTitle.Split('-').First().Split(' ')[2];
                    Console.WriteLine(culture.GetTranslation(TranslationString.RemoveAllSnipe, val), Color.Green);
                }
            }

            Console.WriteLine(culture.GetTranslation(TranslationString.RemoveAllSnipeFinished, plist.Count()), Color.Green);
        }

        public void Runas(string executablepath, string parameters, bool afterKillSelf = true)
        {
            ProcessStartInfo psi = new ProcessStartInfo(Application.ExecutablePath);
            psi.Verb = "runas";
            psi.Arguments = parameters;
            Process.Start(psi);
            if (afterKillSelf)
                Process.GetCurrentProcess().Kill();
        }

        public void Shutdown()
        {
            Shutdown(config.CloseDelaySec);
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
                double lat = double.Parse(latt, CultureInfo.InvariantCulture);
                double lon = double.Parse(lonn, CultureInfo.InvariantCulture);
                Process[] pList = GetNecroBotProcesses();
                for (int i = 0; i < pList.Length; i++)
                {
                    pList[i] = GetProcess(pList[i]);
                    string username = pList[i].MainWindowTitle.Split('-').First().Split(' ')[2];
                    if (!isBotUpperThan094(pList[i].MainModule.FileVersionInfo))
                    {
                        Console.WriteLine(culture.GetTranslation(TranslationString.IncompatibleVersionMsg, username), Color.Red);
                        continue;
                    }
                    string pathRemote = GetSnipeMSPath(Path.GetDirectoryName(pList[i].MainModule.FileName));
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
                            Console.WriteLine(culture.GetTranslation(TranslationString.SendingPokemonToNecroBot,
                                newPokemon.Id.ToLower(),
                                newPokemon.Latitude,
                                newPokemon.Longitude,
                                username), Color.Green);
                        }
                    }
                    else
                    {
                        Console.WriteLine(culture.GetTranslation(TranslationString.AlreadySnipped, newPokemon), Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, Color.Red);
            }
        }

        private static void ExportDefaultTranslation()
        {
            // this method only using for information \temp\languages\translation.en.json
            Translation culture = new Translation();
            culture.Save("en");
            Process.GetCurrentProcess().Kill();
            ///////////////////////////////////////
        }

        private static string GetSnipeMSPath(string NecroBotEXEPath)
        {
            return Path.Combine(NecroBotEXEPath, Variables.SnipeFileName);
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
                _settings = Configs.Load();
            }
            else
            {
                Configs.SaveFiles(_settings);
            }
            return _settings;
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

        private void FileDelete(string path)
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

        private Process GetProcess(Process p)
        {
            int x = 1;
            do
            {
                p = Process.GetProcessById(p.Id);
                Thread.Sleep(1);
                x++;
                if ((x / 1000) > 30)
                    return null;//waiting long time after that throwing exeption, prevent to stucking
            } while ((p.MainWindowTitle.Split(' ').Length == 1));
            return p;
        }

        private bool isBotUpperThan094(FileVersionInfo fversion)
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

        private Translation LoadCulture(Configs _settings)
        {
            return Translation.Load(_settings);
        }
        
    }
}