using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ZeroGInstaller
{
    public class Logger
    {
        public static RichTextBox appLog;
        public static bool useRTB = false;
        public static void StartLogging()
        {
            if (File.Exists("installlog.log"))
            {
                File.Delete("installlog.log");
            }
        }
        public static void LogText(string text)
        {
            File.AppendAllText("installlog.log", text);
            if (useRTB)
            {
                appLog.AppendText(text);
            }
        }
    }
}
