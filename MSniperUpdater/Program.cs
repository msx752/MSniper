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
        private static void CreateEmptyFile(string fullpath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(fullpath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullpath));
            }
            if (!File.Exists(fullpath))
            {
                StreamWriter sw = new StreamWriter(fullpath, false);
                sw.Write(' ');
                sw.Close();
            }
        }

        private static void DecompressZip(string zipFullPath, string NameWithVersion)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipFullPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string path = Path.Combine(NameWithVersion, entry.FullName);
                    CreateEmptyFile(path);
                    entry.ExtractToFile(path, true);
                }
            }
        }
        static void Main(string[] args)
        {
            args = new string[] { "MSniper.v1.0.5" };
            if (args.Length > 0)
            {
                Process[] plist = Process.GetProcessesByName("msniper");
                for (int i = 0; i < plist.Length; i++)
                {
                    try
                    {
                        Process.GetProcessById(plist[i].Id).Kill();
                    }
                    catch { }
                }

                string[] deleteList = new string[] {
                        "MSniper.exe",
                        "Newtonsoft.Json.dll",
                        "registerProtocol.bat",
                        "removeProtocol.bat",
                        "resetSnipeList.bat"
                    };
                foreach (var item in deleteList)
                {
                    string delete = Path.Combine(Application.StartupPath, item);
                    File.Delete(delete);
                }
                string[] CopyList = Directory.GetFiles(Application.StartupPath, $"temp\\{args[0]}\\");
                foreach (var item in CopyList)
                {
                    FileInfo f = new FileInfo(item);
                    string newDir = Path.Combine(Application.StartupPath, f.Name);
                    File.Copy(f.FullName, newDir);
                }
                string run_exe = Path.Combine(Application.StartupPath, "msniper.exe");
                Task.Run(() =>
                {
                    ProcessStartInfo psi = new ProcessStartInfo(run_exe);
                    Process proc = new Process();
                    proc.StartInfo = psi;
                    proc.Start();
                    proc.WaitForExit();
                });
                string temp_FOlder = Directory.GetParent(CopyList[0]).ToString();
                try
                {
                    Directory.Delete(temp_FOlder,true);
                }
                catch
                {
                    Directory.Delete(temp_FOlder, true);
                }
                Thread.Sleep(1000);
                Process.GetCurrentProcess().Kill();
            }

            Console.ReadLine();
        }
    }
}
