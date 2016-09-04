using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Console;

namespace MSniperUpdater
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //args = new string[] { "MSniper.v1.0.5" };
            if (args.Length > 0)
            {
                var plist = Process.GetProcessesByName("msniper");
                WriteLine("killing msniper.exe....");
                foreach (var t in plist)
                {
                    try
                    {
                        while (true)
                        {
                            for (var i2 = 0; i2 < 10; i2++)
                            {
                                Thread.Sleep(200);
                                t.Kill();
                                break;
                            }
                            break;
                        }

                    }
                    catch { }
                }

                Thread.Sleep(500);
                var deleteList = new string[] {
                        "msniper.exe",
                        "Newtonsoft.Json.dll",
                        "registerProtocol.bat",
                        "removeProtocol.bat",
                        "resetSnipeList.bat"
                    };
                WriteLine("deleting old files....");
                foreach (var item in deleteList)
                {
                    try
                    {
                        while (true)
                        {
                            for (var i2 = 0; i2 < 10; i2++)
                            {
                                var delete = Path.Combine(Application.StartupPath, item);
                                Thread.Sleep(200);
                                File.Delete(delete);
                                break;
                            }
                            break;
                        }
                    }
                    catch { }
                }
                WriteLine("copying NEW files....");
                Thread.Sleep(300);
                var copyList = Directory.GetFiles(Application.StartupPath, $"temp\\{args[0]}\\");
                foreach (var item in copyList)
                {
                    var f = new FileInfo(item);
                    var newDir = Path.Combine(Application.StartupPath, f.Name);
                    File.Copy(f.FullName, newDir, true);
                }
                WriteLine("TEMP DELETING....");
                Thread.Sleep(100);
                var tempFolder = Directory.GetParent(Directory.GetParent(copyList[0]).ToString()).ToString();
                try
                {
                    Directory.Delete(tempFolder, true);
                }
                catch
                {
                    Directory.Delete(tempFolder, true);
                }
                WriteLine("STARTING MSNIPER....");
                var runExe = Path.Combine(Application.StartupPath, "msniper.exe");
                Task.Run(() =>
                {
                    var psi = new ProcessStartInfo(runExe);
                    var proc = new Process {StartInfo = psi};
                    proc.Start();
                    proc.WaitForExit();
                });
                Thread.Sleep(2000);
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                WriteLine("\t runs with parameter");
                WriteLine("\t runs with parameter");
            }

            //Console.ReadLine();
        }
    }
}
