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
        private static string botEXEname = "necrobot";
        private static string githupProjectLink = "https://github.com/msx752/MSniper";
        public static string protocolName = "pokesniper2";
        private static string snipefilename = "SnipeMS.json";
        public static string VersionUri = "https://raw.githubusercontent.com/msx752/MSniper/master/MSniper/Properties/AssemblyInfo.cs";
        private static string RequireVersion = "0.9.5";

        private static void Helper()
        {
            Log.WriteLine("");
            Log.WriteLine("MSniper - NecroBot Manual PokemonSniper by msx752");
            Log.WriteLine("GitHub Project " + githupProjectLink, ConsoleColor.Yellow);
            Log.Write("Current Version: " + Assembly.GetEntryAssembly().GetName().Version.ToString(), ConsoleColor.White);
            if (VersionCheck.IsLatest())
            {
                Log.WriteLine("   * Latest Version *", ConsoleColor.White);
            }
            else
            {
                Log.WriteLine(string.Format("   * New Version: {0} *", VersionCheck.RemoteVersion), ConsoleColor.Green);
                Log.WriteLine(string.Format("* DOWNLOAD LINK:  {0}/{1} *", githupProjectLink, "releases/latest"), ConsoleColor.Yellow);
                Shutdown(5);
            }
            Log.WriteLine("MSniper integrated NecroBot v0.9.5 or upper", ConsoleColor.DarkCyan);
            Log.WriteLine("--------------------------------------------------------");
            Console.WriteLine("");
        }

        private static void Main(string[] args)
        {
            Console.Clear();
            Helper();
            if (Process.GetProcessesByName(botEXEname).Count() == 0)
            {
                Log.WriteLine("Any running NecroBot not found...", ConsoleColor.Red);
                Log.WriteLine(" *Necrobot must be running before MSniper*", ConsoleColor.Red);
            }
            //args = new string[] { "pokesniper2://Dragonite/37.766627,-122.403677" };//for debug mode
            if (args.Length == 1)
            {
                switch (args.First())
                {
                    case "-register":
                        Runas(Application.ExecutablePath, "-registerp");
                        break;

                    case "-registerp":
                        ProtocolRegister.RegisterUrl(protocolName);
                        Log.WriteLine("Protocol Registered:", ConsoleColor.Green);
                        break;

                    case "-remove":
                        Runas(Application.ExecutablePath, "-removep");
                        break;

                    case "-removep":
                        ProtocolRegister.DeleteUrl(protocolName);
                        Log.WriteLine("Protocol Deleted:", ConsoleColor.Red);
                        break;

                    case "-resetallsnipelist":
                        RemoveAllSnipeMSJSON();
                        break;

                    default:
                        string re1 = "(pokesniper2://)";//protocol
                        string re6 = "((?:[a-z][a-z]+))";//pokemon name
                        string re7 = "(\\/)";
                        string re8 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lat
                        string re9 = "(,)";
                        string re10 = "([+-]?\\d*\\.\\d+)(?![-+0-9\\.])";//lon

                        Regex r = new Regex(re1 + re6 + re7 + re8 + re9 + re10, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                        Match m = r.Match(args.First());
                        if (m.Success)
                        {
                            Snipe(m.Groups[1].ToString(), m.Groups[3].ToString(), m.Groups[5].ToString());
                        }
                        else
                        {
                            Log.WriteLine("unknown format", ConsoleColor.Red);
                        }
                        break;
                }
            }
            else if (args.Length == 0)
            {
                Log.WriteLine("Paste Format=> PokemonName Latitude,Longitude", ConsoleColor.DarkGray);
                do
                {
                    Log.WriteLine("waiting user data..", ConsoleColor.White);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    string snipping = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    //snipping = "Dragonite 37.766627,-122.403677";//for debug mode
                    if (snipping.ToLower() == "e")
                        break;
                    string re1 = "((?:[a-z][a-z]+))";//pokemon name
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
            Shutdown(4);
        }

        private static void RemoveAllSnipeMSJSON()
        {
            foreach (var item in Process.GetProcessesByName(botEXEname))
            {
                string path = Path.Combine(Path.GetDirectoryName(item.MainModule.FileName), snipefilename);
                if (File.Exists(path))
                {
                    FileDelete(path);
                    string val = item.MainWindowTitle.Split('-').First().Split(' ')[2];
                    Log.WriteLine(string.Format("deleted {0} for {1} :", snipefilename, val), ConsoleColor.Green);
                }
            }
            Log.WriteLine("deleting finished..", ConsoleColor.Green);
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
            Log.WriteLine("Program is closing in " + seconds + "sec");
            Thread.Sleep(seconds * 1000);
            Process.GetCurrentProcess().Kill();
        }

        private static bool isBotUpperThan094(FileVersionInfo fversion)
        {
            if (new Version(fversion.FileVersion) >= new Version(RequireVersion))
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
                foreach (var item in Process.GetProcessesByName(botEXEname))
                {
                    string username = item.MainWindowTitle.Split('-').First().Split(' ')[2];
                    if (!isBotUpperThan094(item.MainModule.FileVersionInfo))
                    {
                        Log.WriteLine(string.Format("Incompatible NecroBot version for {0}", username), ConsoleColor.Red);
                        continue;
                    }
                    string path = Path.Combine(Path.GetDirectoryName(item.MainModule.FileName), snipefilename);
                    List<MSniperInfo> MSniperLocation = ReadSnipeMS(path);
                    MSniperInfo newPokemon = new MSniperInfo()
                    {
                        Latitude = lat,
                        Longitude = lon,
                        Id = pokemonName
                    };
                    if (MSniperLocation.FindIndex(p => p.Id == newPokemon.Id && p.Latitude == newPokemon.Latitude && p.Longitude == newPokemon.Longitude) == -1)
                    {
                        MSniperLocation.Add(newPokemon);
                        if (WriteSnipeMS(MSniperLocation, newPokemon, path))
                        {
                            Log.WriteLine(string.Format("Sending to {3}: {0} {1},{2}",
                                newPokemon.Id,
                                newPokemon.Latitude,
                                newPokemon.Longitude,
                                username), ConsoleColor.Green);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message, ConsoleColor.DarkRed);
            }
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