using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MSniper
{
    public static class Protocol
    {
        private static readonly List<string> Protocols = new List<string>() { "msniper", "pokesniper2" };
        public static void Delete()
        {
            try
            {
                foreach (var protocolName in Protocols)
                {
                    if (Registry.ClassesRoot.OpenSubKey(protocolName) != null)
                        Registry.ClassesRoot.DeleteSubKeyTree(protocolName, true);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool IsRegistered()
        {
            return Protocols.All(protocolName => Registry.ClassesRoot.OpenSubKey(protocolName) != null);
        }

        //not necessary
        //public static void isProtocolExists(string protocolName)
        //{
        //    if (Registry.ClassesRoot.OpenSubKey(protocolName) == null)
        //    {
        //        RegisterUrl(protocolName);
        //    }
        //    else
        //    {
        //        DeleteUrl(protocolName);
        //        RegisterUrl(protocolName);
        //    }
        //}

        public static void Register()
        {
            try
            {
                foreach (var protocolName in Protocols)
                {
                    var nkey = Registry.ClassesRoot.CreateSubKey(protocolName);
                    nkey.SetValue(null, $"URL:{protocolName} protocol");
                    nkey.SetValue("URL Protocol", string.Empty);
                    Registry.ClassesRoot.CreateSubKey(protocolName + "\\Shell");
                    Registry.ClassesRoot.CreateSubKey(protocolName + "\\Shell\\open");
                    nkey = Registry.ClassesRoot.CreateSubKey(protocolName + "\\Shell\\open\\command");

                    nkey.SetValue(null, "\"" + Application.ExecutablePath + "\" \"%1\"");
                    nkey.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}