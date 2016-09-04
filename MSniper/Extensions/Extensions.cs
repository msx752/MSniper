using System.Diagnostics;
using System.Linq;

namespace MSniper.Extensions
{
    public static class Extensions
    {
        public static string GetWindowTitle(this Process p)
        {
            try
            {
                var title = Process.GetProcessById(p.Id).MainWindowTitle;
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
