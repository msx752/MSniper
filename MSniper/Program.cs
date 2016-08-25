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
        public static string versionUri = "https://raw.githubusercontent.com/msx752/MSniper/master/MSniper/Properties/AssemblyInfo.cs";
        public static string botEXEName = "necrobot";
        public static string githupProjectLink = "https://github.com/msx752/MSniper";
        public static string snipefilename = "SnipeMS.json";
        public static string minRequireVersion = "0.9.5";

        [STAThread]
        private static void Main(string[] args)
        {
            ExportReferences();
            Console.Title = string.Format("MSniper v{0}    by msx752", Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5));
            Console.Clear();
            Helper(args.Length == 0 ? false : true);
            if (Process.GetProcessesByName(botEXEName).Count() == 0)
            {
                Log.WriteLine("Any running NecroBot not found...", ConsoleColor.Red);
                Log.WriteLine(" *Necrobot must be running before MSniper*", ConsoleColor.Red);
                if (args.Length != 1)
                    Shutdown(5);
            }
            //args = new string[] { "pokesniper2://Ivysaur/-33.890835,151.223859" };//for debug mode
            if (args.Length == 1)
            {
                switch (args.First())
                {
                    case "-register":
                        Runas(Application.ExecutablePath, "-registerp");
                        break;

                    case "-registerp":
                        Protocol.Register();
                        Log.WriteLine("Protocol Successfully REGISTERED:", ConsoleColor.Green);
                        break;

                    case "-remove":
                        Runas(Application.ExecutablePath, "-removep");
                        break;

                    case "-removep":
                        Protocol.Delete();
                        Log.WriteLine("Protocol Successfully DELETED:", ConsoleColor.Green);
                        break;

                    case "-resetallsnipelist":
                        RemoveAllSnipeMSJSON();
                        break;

                    default:
                        string re0 = "(pokesniper2://)";//protocol
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
                            Log.WriteLine("unknown link format", ConsoleColor.Red);
                        }
                        break;
                }
            }
            else if (args.Length == 0)
            {
                Log.WriteLine("\t\tCUSTOM PASTE ACTIVE", ConsoleColor.DarkGray);
                Log.WriteLine("format: PokemonName Latitude,Longitude", ConsoleColor.DarkGray);
                Log.WriteLine("--------------------------------------------------------");
                do
                {
                    Log.WriteLine("waiting data..", ConsoleColor.White);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    string snipping = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    //snipping = "Dragonite 37.766627,-122.403677";//for debug mode
                    if (snipping.ToLower() == "e")
                        break;
                    string re1 = "((?:\\w+))";//pokemon name
                    string re2 = "( )";//separator
                    string re3 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lat
                    string re4 = "(,)";//separator
                    string re5 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lon
                    Regex r = new Regex(re1 + re2 + re3 + re4 + re5, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match m = r.Match(snipping);
                    if (m.Success)
                    {
                        Snipe(m.Groups[1].ToString(), m.Groups[3].ToString(), m.Groups[5].ToString());
                    }
                    else
                    {
                        Log.WriteLine("wrong format retry or write 'E' for quit..", ConsoleColor.Red);
                    }
                }
                while (true);
            }
            Shutdown(5);
        }

        private static void Helper(bool withParams)
        {
            Log.WriteLine("");
            Log.WriteLine("MSniper - NecroBot Manual PokemonSniper by msx752");
            Log.WriteLine("GitHub Project " + githupProjectLink, ConsoleColor.Yellow);
            Log.Write("Current Version: " + Assembly.GetEntryAssembly().GetName().Version.ToString().Substring(0, 5), ConsoleColor.White);
            if (!Protocol.isRegistered() && withParams == false)
            {
                Log.WriteLine(" ");
                Log.WriteLine("Protocol not found - Please run once registerProtocol.bat", ConsoleColor.Red);
                Shutdown(5);
            }
            if (VersionCheck.IsLatest())
            {
                Log.WriteLine("   * Latest Version *", ConsoleColor.White);
            }
            else
            {
                Log.WriteLine(string.Format("   * NEW VERSION: {0} *", VersionCheck.RemoteVersion.ToString().Substring(0, 5)), ConsoleColor.Green);
                string downloadlink = githupProjectLink + "/releases/latest";
                Log.WriteLine(string.Format("* DOWNLOAD LINK:  {0} *", downloadlink), ConsoleColor.Yellow);
                Log.WriteLine(string.Format("PRESS 'C' TO COPY LINK OR PRESS ANY KEY FOR EXIT.."), ConsoleColor.DarkCyan);
                char c = Console.ReadKey().KeyChar;
                Console.SetCursorPosition(0, Console.CursorTop);
                if (c == 'c' || c == 'C')
                {
                    Clipboard.SetText(downloadlink);
                    Log.WriteLine(string.Format("link successfully copied.."), ConsoleColor.Green);
                }
                Shutdown(5);
            }
            Log.WriteLine(string.Format("MSniper integrated NecroBot v{0} or upper", minRequireVersion), ConsoleColor.DarkCyan);
            Log.WriteLine("--------------------------------------------------------");
        }

        private static void RemoveAllSnipeMSJSON()
        {
            Process[] plist = Process.GetProcessesByName(botEXEName);
            foreach (var item in plist)
            {
                string pathRemote = GetSnipeMSLocation(Path.GetDirectoryName(item.MainModule.FileName));
                if (File.Exists(pathRemote))
                {
                    FileDelete(pathRemote);
                    string val = item.MainWindowTitle.Split('-').First().Split(' ')[2];
                    Log.WriteLine(string.Format("deleted {0} for {1}", snipefilename, val), ConsoleColor.Green);
                }
            }
            Log.WriteLine(string.Format("deleting finished process count:{0}..", plist.Count()), ConsoleColor.Green);
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

        private static void Shutdown(int seconds)
        {
            for (int i = seconds; i >= 0; i--)
            {
                Log.WriteLine("Program is closing in " + (i + 1) + "sec");
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
            if (new Version(fversion.FileVersion) >= new Version(minRequireVersion))
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
                foreach (var item in Process.GetProcessesByName(botEXEName))
                {
                    string username = item.MainWindowTitle.Split('-').First().Split(' ')[2];
                    if (!isBotUpperThan094(item.MainModule.FileVersionInfo))
                    {
                        Log.WriteLine(string.Format("Incompatible NecroBot version for {0}", username), ConsoleColor.Red);
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
                            Log.WriteLine(string.Format("Sending to {3}: {0} {1},{2}",
                                newPokemon.Id.ToLower(),
                                newPokemon.Latitude,
                                newPokemon.Longitude,
                                username), ConsoleColor.Green);
                        }
                    }
                    else
                    {
                        Log.WriteLine(string.Format("{0}\t\tAlready Snipped...", newPokemon), ConsoleColor.DarkRed);
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
        }

        private static string GetSnipeMSLocation(string NecroBotEXEPath)
        {
            return Path.Combine(NecroBotEXEPath, snipefilename);
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