using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSniper
{
    public static class Log
    {
        public static void WriteLine(string msg, ConsoleColor cclr = ConsoleColor.Gray)
        {
            Console.ForegroundColor = cclr;
            Console.WriteLine(" " + msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void Write(string msg, ConsoleColor cclr = ConsoleColor.Gray)
        {
            Console.ForegroundColor = cclr;
            Console.Write(" " + msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
