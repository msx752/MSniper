using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MSniper
{
    class Program
    {
        static string botname = "necrobot";
        static string snipefilename = "SnipeMS.json";

        static void Main(string[] args)
        {
            Console.Clear();
            helper();
            if (Process.GetProcessesByName(botname).Count() == 0)
                Log.WriteLine("Any running NecroBot not found...", ConsoleColor.Red);

            if (args.Length == 1)
            {
                //args[0] = "pokesniper2://Dragonite/37.766627,-122.403677";
                ProcessStartInfo psi = null;
                switch (args.First())
                {
                    case "-register":
                        psi = new ProcessStartInfo(Application.ExecutablePath);
                        psi.Verb = "runas";
                        psi.Arguments = "-registerp";
                        Process.Start(psi);
                        Process.GetCurrentProcess().Kill();
                        break;

                    case "-registerp":
                        ProtocolRegister.RegisterUrl();
                        Log.WriteLine("Protocol Registered:", ConsoleColor.Green);
                        break;

                    case "-remove":
                        psi = new ProcessStartInfo(Application.ExecutablePath);
                        psi.Verb = "runas";
                        psi.Arguments = "-removep";
                        Process.Start(psi);
                        Process.GetCurrentProcess().Kill();
                        break;

                    case "-removep":
                        ProtocolRegister.DeleteUrl();
                        Log.WriteLine("Protocol Deleted:", ConsoleColor.Red);
                        break;

                    case "-resetallsnipelist":
                        foreach (var item in Process.GetProcessesByName(botname))
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
                        break;

                    default:
                        string[] pars = args[0].Split('/');//0:protocol,1:null,2:pokemonname,3:geocoords

                        if (pars.Length != 4)//pokesniper2://Dragonite/37.766627,-122.403677
                        {
                            Log.WriteLine("unknown format", ConsoleColor.Red);
                            break;
                        }
                        string[] coord = pars[3].Split(',');//0:lat,1:lon
                        double lat = double.Parse(coord[0], CultureInfo.InvariantCulture);
                        double lon = double.Parse(coord[1], CultureInfo.InvariantCulture);
                        string pokemonName = pars[2];
                        foreach (var item in Process.GetProcessesByName(botname))
                        {
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
                                } while (true);
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
                                        string val = item.MainWindowTitle.Split('-').First().Split(' ')[2];
                                        Log.WriteLine(string.Format("Sending to {3}: {0} {1},{2}", pokemonName, lat, lon, val), ConsoleColor.Green);
                                        break;
                                    }
                                    catch { Thread.Sleep(200); }

                                } while (true);
                            }
                        }
                        break;
                }
            }
            Shutdown(2);
        }
        static void FileDelete(string path)
        {
            do
            {
                try
                {
                    File.Delete(path);
                    break;
                }
                catch { }
            } while (true);
        }
        static void helper()
        {
            Log.WriteLine("");
            Log.WriteLine("MSniper - NecroBot Pokemon Sniper by msx752");
            Log.Write("Current Version: " + Assembly.GetEntryAssembly().GetName().Version.ToString(), ConsoleColor.White);
            if (true)
            {
                Log.WriteLine("");
            }
            else
            {
                Log.WriteLine("   *new version up*", ConsoleColor.Green);
            }
            Log.WriteLine("--------------------------------------------------------");
            Console.WriteLine("");
        }

        static void Shutdown(int seconds)
        {
            Log.WriteLine("Program is closing in " + seconds + "sec");
            Thread.Sleep(seconds * 1000);
            Application.ExitThread();
        }
    }
}
