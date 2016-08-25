using System;

namespace MSniper
{
    public static class Log
    {
        public static int currentLineNumber = -1;
        public static int lastLineCount = 0;
        public static void Write(string msg, ConsoleColor cclr = ConsoleColor.Gray)
        {
            Console.ForegroundColor = cclr;
            Console.Write(" " + msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void WriteLine(string msg, ConsoleColor cclr = ConsoleColor.Gray)
        {
            Write(msg + Environment.NewLine, cclr);
            currentLineNumber++;
            lastLineCount = msg.Length+1;
        }
    }
}