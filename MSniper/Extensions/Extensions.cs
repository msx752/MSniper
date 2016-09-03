using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSniper
{
    public static class Extensions
    {
        public static string GetWindowTitle(this Process p)
        {
            try
            {
                string title = Process.GetProcessById(p.Id).MainWindowTitle;
                try
                {
                    return title.Split('-').First().Split(' ')[2];
                }
                catch
                {
                    return title;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
