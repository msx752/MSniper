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
using System.Threading;
using System.Windows.Forms;

namespace MSniper
{
    internal class Program
    {
        public static string botEXEname = "necrobot";
        public static string githupProjectLink = "https://github.com/msx752/MSniper";
        public static string protocolName = "pokesniper2";
        public static string snipefilename = "SnipeMS.json";
        public static string VersionUri = "https://raw.githubusercontent.com/msx752/MSniper/master/MSniper/Properties/AssemblyInfo.cs";

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

        private static void Helper()
        {
            Log.WriteLine("");
            Log.WriteLine("MSniper - NecroBot Manual PokemonSniper by msx752");
            Log.WriteLine("GitHub Project " + githupProjectLink, ConsoleColor.Yellow);
            Log.Write("Current Version: " + Assembly.GetEntryAssembly().GetName().Version.ToString(), ConsoleColor.White);
            if (VersionCheck.IsLatest())
            {
                Log.WriteLine("   * lastet version *", ConsoleColor.White);
            }
            else
            {
                Log.WriteLine(string.Format("   * New Version: {0} *",VersionCheck.RemoteVersion), ConsoleColor.Green);
                Log.WriteLine(string.Format("* Loot at {0}/{1} *", githupProjectLink, "releases"), ConsoleColor.Yellow);
            }
            Log.WriteLine("MSniper integrated NecroBot v0.9.5 and upper", ConsoleColor.DarkCyan);
            Log.WriteLine("--------------------------------------------------------");
            Console.WriteLine("");
            if (Process.GetProcessesByName(botEXEname).Count() == 0)
            {
                Log.WriteLine("Any running NecroBot not found...", ConsoleColor.Red);
                Log.WriteLine(" *Necrobot must be running before MSniper*", ConsoleColor.Red);
            }
        }

        private static bool isBotUpperThan094(FileVersionInfo fversion)
        {
            if (new Version(fversion.FileVersion) >= new Version("0.9.5"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void Main(string[] args)
        {
            Console.Clear();
            Helper();
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
                        SnipePokemon(args);
                        break;
                }
            }
            Shutdown(5);
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

        private static void Shutdown(int seconds)
        {
            Log.WriteLine("Program is closing in " + seconds + "sec");
            Thread.Sleep(seconds * 1000);
            Application.ExitThread();
        }

        private static void SnipePokemon(string[] parameters)
        {
            try
            {
                string[] pars = parameters[0].Split('/');//0:protocol,1:null,2:pokemonname,3:geocoords

                if (pars.Length != 4)//pokesniper2://Dragonite/37.766627,-122.403677
                {
                    Log.WriteLine("unknown format", ConsoleColor.Red);
                    return;
                }
                string[] coord = pars[3].Split(',');//0:lat,1:lon
                double lat = double.Parse(coord[0], CultureInfo.InvariantCulture);
                double lon = double.Parse(coord[1], CultureInfo.InvariantCulture);
                string pokemonName = pars[2];
                foreach (var item in Process.GetProcessesByName(botEXEname))
                {
                    string username = item.MainWindowTitle.Split('-').First().Split(' ')[2];
                    if (!isBotUpperThan094(item.MainModule.FileVersionInfo))
                    {
                        Log.WriteLine(string.Format("Incompatible NecroBot version for {0}", username), ConsoleColor.Red);
                        continue;
                    }
                    isBotUpperThan094(item.MainModule.FileVersionInfo);
                    List<MSniperInfo> MSniperLocation = new List<MSniperInfo>();
                    string path = Path.Combine(Path.GetDirectoryName(item.MainModule.FileName), snipefilename);
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
                        } while (true);//waiting access
                        MSniperLocation = JsonConvert.DeserializeObject<List<MSniperInfo>>(jsn);
                    }
                    if (MSniperLocation.FindIndex(p => p.Id == pokemonName && p.Latitude == lat && p.Longitude == lon) == -1)
                    {
                        MSniperLocation.Add(new MSniperInfo()
                        {
                            Latitude = lat,
                            Longitude = lon,
                            Id = pokemonName
                        });
                        do
                        {
                            try
                            {
                                StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8);
                                sw.WriteLine(JsonConvert.SerializeObject(
                                    MSniperLocation,
                                    Formatting.Indented,
                                    new StringEnumConverter { CamelCaseText = true }));
                                sw.Close();
                                Log.WriteLine(string.Format("Sending to {3}: {0} {1},{2}", pokemonName, lat, lon, username), ConsoleColor.Green);
                                break;
                            }
                            catch { Thread.Sleep(200); }
                        } while (true);//waiting access
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message, ConsoleColor.DarkRed);
            }
        }
    }
}