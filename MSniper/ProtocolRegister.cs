using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MSniper
{
    public static class Protocol
    {
        private static List<string> protocols = new List<string>() { "msniper", "pokesniper2" };
        public static void Delete()
        {
            try
            {
                foreach (var protocolName in protocols)
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

        public static bool isRegistered()
        {
            foreach (var protocolName in protocols)
            {
                if (Registry.ClassesRoot.OpenSubKey(protocolName) == null)
                {
                    return false;
                }
            }
            return true;
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
                foreach (var protocolName in protocols)
                {
                    RegistryKey nkey = Registry.ClassesRoot.CreateSubKey(protocolName);
                    nkey.SetValue(null, string.Format("URL:{0} protocol", protocolName));
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