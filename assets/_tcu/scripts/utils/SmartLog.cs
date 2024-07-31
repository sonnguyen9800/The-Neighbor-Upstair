using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCU
{
    internal static class SmartLog
    {
        public enum LogLevel
        {
            Info,
            Warn, 
            Error
        }
        public enum LogColor
        {
            White,
            Green,
            Yellow,
            Red
        }
        public static void PrintError(string message)
        {
            GD.PrintErr(string.Format("[Error] {0}", message));

        }

        public static void Print(string message, LogLevel logLevel = LogLevel.Info, LogColor logColor = LogColor.White)
        {
            if (logColor == LogColor.White)
            {
                GD.Print(string.Format("[{0}] {1}", logLevel.ToString(), message));
                if (logLevel == LogLevel.Error)
                {
                    GD.PrintErr(string.Format("[{0}] {1}", logLevel.ToString(), message));

                }
                return;
            }else
            {
                GD.PrintRich(string.Format("[{0}] [color={2}]{1}[/color]", logLevel.ToString(), message, logColor.ToString().ToLower()));
                return;

            }
        }
    }
}
