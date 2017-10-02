using System;
using LogsLib;
using NLog;

namespace LoLApiParser
{
    public static class LoggersAdapter
    {
        private static readonly Logger notifLogger = LogExManager.GetLogger(new LogExKey
        {
            Targets = new[] { LogExTargetType.Console, LogExTargetType.KbLog, },
            LogName = "Notification",
            ProjectName = "LoLApiParser",
            KbLogIndexName = "cyber",
        });

        public static void Debug(string message, params object[] args)
        {
            notifLogger.Debug(message, args);
        }

        public static void Info(string message, params object[] args)
        {
            notifLogger.Info(message, args);
        }

        public static void Error(Exception exception, string message = "", params object[] args)
        {
            notifLogger.Error(exception, message, args);
        }

        public static void Error(string message = "", params object[] args)
        {
            notifLogger.Error(message, args);
        }

        public static void Warn(string message, params object[] args)
        {
            notifLogger.Warn(message, args);
        }
    }
}
