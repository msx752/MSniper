using System;

namespace MSniper
{
    public static class Log
    {
        public static void Write(string msg, ConsoleColor cclr = ConsoleColor.Gray)
        {
            Console.ForegroundColor = cclr;
            Console.Write(" " + msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void WriteLine(string msg, ConsoleColor cclr = ConsoleColor.Gray)
        {
            Write(msg + Environment.NewLine, cclr);
        }
    }
}