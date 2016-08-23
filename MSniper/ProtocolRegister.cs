using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace MSniper
{
    public static class ProtocolRegister
    {
        public static void DeleteUrl(string protocolName)
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

        public static void RegisterUrl(string protocolName)
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