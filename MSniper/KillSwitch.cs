using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MSniper
{
    public class KillSwitch
    {
        /// <summary>
        /// reference from
        /// https://github.com/Necrobot-Private/NecroBot/blob/master/PoGo.NecroBot.CLI/Program.cs
        /// </summary>
        /// <returns></returns>
        public static bool CheckKillSwitch()
        {
            try
            {
                var strResponse = Downloader.DownloadString(Variables.StrKillSwitchUri);

                if (strResponse == null)
                    return false;

                var strSplit = strResponse.Split(';');

                if (strSplit.Length > 1)
                {
                    var strStatus = strSplit[0];
                    var strReason = strSplit[1];

                    if (strStatus.Contains("DISABLED"))
                    {
                        Program.frm.Console.WriteLine(strReason + $"\n");

                        Program.frm.Console.WriteLine("The msniper will now close, please press enter to continue", System.Drawing.Color.Wheat);
                        Program.frm.Console.ReadLine();
                        Program.frm.Shutdown(1);
                        return true;
                    }
                }
                else
                    return false;
            }
            catch (WebException)
            {
                // ignored
            }
            return false;
        }
    }
}
