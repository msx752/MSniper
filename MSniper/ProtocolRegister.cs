using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace MSniper
{
    public static class Protocol
    {
        public static string protocolName = "pokesniper2";
        public static void Delete()
        {
            try
            {
                if (Registry.ClassesRoot.OpenSubKey(protocolName) != null)
                    Registry.ClassesRoot.DeleteSubKeyTree(protocolName, true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool isRegistered()
        {
            if (Registry.ClassesRoot.OpenSubKey(protocolName) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
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
                RegistryKey nkey = Registry.ClassesRoot.CreateSubKey(protocolName);
                nkey.SetValue(null, string.Format("URL:{0} protocol", protocolName));
                nkey.SetValue("URL Protocol", string.Empty);
                Registry.ClassesRoot.CreateSubKey(protocolName + "\\Shell");
                Registry.ClassesRoot.CreateSubKey(protocolName + "\\Shell\\open");
                nkey = Registry.ClassesRoot.CreateSubKey(protocolName + "\\Shell\\open\\command");

                nkey.SetValue(null, "\"" + Application.ExecutablePath + "\" \"%1\"");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}