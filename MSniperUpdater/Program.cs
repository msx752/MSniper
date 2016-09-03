using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSniperUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[] { "MSniper.v1.0.5" };
            if (args.Length > 0)
            {
                Process[] plist = Process.GetProcessesByName("msniper");
                Console.WriteLine("killing msniper.exe....");
                for (int i = 0; i < plist.Length; i++)
                {
                    try
                    {
                        while (true)
                        {
                            for (int i2 = 0; i2 < 10; i2++)
                            {
                                Thread.Sleep(200);
                                plist[i].Kill();
                                break;
                            }
                            break;
                        }

                    }
                    catch { }
                }

                Thread.Sleep(500);
                string[] deleteList = new string[] {
                        "msniper.exe",
                        "Newtonsoft.Json.dll",
                        "registerProtocol.bat",
                        "removeProtocol.bat",
                        "resetSnipeList.bat"
                    };
                Console.WriteLine("deleting old files....");
                foreach (var item in deleteList)
                {
                    try
                    {
                        while (true)
                        {
                            for (int i2 = 0; i2 < 10; i2++)
                            {
                                string delete = Path.Combine(Application.StartupPath, item);
                                Thread.Sleep(200);
                                File.Delete(delete);
                                break;
                            }
                            break;
                        }

                    }
                    catch { }
                }
                Console.WriteLine("copying NEW files....");
                Thread.Sleep(300);
                string[] CopyList = Directory.GetFiles(Application.StartupPath, $"temp\\{args[0]}\\");
                foreach (var item in CopyList)
                {
                    FileInfo f = new FileInfo(item);
                    string newDir = Path.Combine(Application.StartupPath, f.Name);
                    File.Copy(f.FullName, newDir, true);
                }
                Console.WriteLine("TEMP DELETING....");
                Thread.Sleep(100);
                string temp_FOlder = Directory.GetParent(Directory.GetParent(CopyList[0]).ToString()).ToString();
                try
                {
                    Directory.Delete(temp_FOlder, true);
                }
                catch
                {
                    Directory.Delete(temp_FOlder, true);
                }
                Console.WriteLine("STARTING MSNIPER....");
                string run_exe = Path.Combine(Application.StartupPath, "msniper.exe");
                Task.Run(() =>
                {
                    ProcessStartInfo psi = new ProcessStartInfo(run_exe);
                    Process proc = new Process();
                    proc.StartInfo = psi;
                    proc.Start();
                    proc.WaitForExit();
                });
                Thread.Sleep(2000);
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                Console.WriteLine("\t runs with parameter");
                Console.WriteLine("\t runs with parameter");
            }

            //Console.ReadLine();
        }
    }
}
