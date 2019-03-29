using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZeroG.Logger
{
    public enum LogSeverity
    {
        Error = 0,
        Debug,
        Verbose,
        General
    }

    public static class WriteLog
    {
        private static LogSeverity currentLogLevel;
        private static bool enabled = true;
        public static void SetLogLevel(LogSeverity logLevel, bool restart)
        {
            currentLogLevel = logLevel;
            if (restart && File.Exists("zerog.log") && enabled)
            {
                File.Delete("zerog.log");
            }
        }
        public static void EnableDisable(bool enabling)
        {
            enabled = enabling;
        }
        public static void Error(string logMessage)
        {
            WriteToLog(LogSeverity.Error, logMessage);
        }
        public static void Debug(string logMessage)
        {
            WriteToLog(LogSeverity.Debug, logMessage);
        }
        public static void Verbose(string logMessage)
        {
            WriteToLog(LogSeverity.Verbose, logMessage);
        }
        public static void General(string logMessage)
        {
            WriteToLog(LogSeverity.General, logMessage);
        }
        private static void WriteToLog(LogSeverity messageLevel, string logMessage)
        {
            if (messageLevel <= currentLogLevel)
            {
                string log = DateTime.Now + "    " + messageLevel.ToString() + ":" + logMessage;
                Console.WriteLine(log);
                File.AppendAllText("zerog.log", log + Environment.NewLine);
            }
        }
    }
}
