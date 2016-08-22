using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Forms;

namespace MSniper
{
    public static class ProtocolRegister
    {
        public static string protocolName { get; set; } = "pokesniper2";
        public static void RegisterUrl()
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
        public static void DeleteUrl()
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

        public static void isProtocolExists()
        {
            if (Registry.ClassesRoot.OpenSubKey(protocolName) == null)
            {
                RegisterUrl();
            }
            else
            {
                DeleteUrl();
                RegisterUrl();
            }
        }
    }
}
