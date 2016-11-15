using MSniper;
using MSniper.Settings;
using MSniper.Settings.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSniper.WFConsole;

namespace MSniper
{
    static class Program
    {
        public static FWindow frm { get; set; }
        #region form
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] _args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frm = new FWindow {Console = {Arguments = _args}};
            if (_args.Length > 0)
                frm.WindowState = FormWindowState.Minimized;
            Application.Run(frm);
        }
        #endregion
    }
}
