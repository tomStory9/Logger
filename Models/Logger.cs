using System;
using System.IO;
using LoggerDLL.Models;
namespace LoggerDLL.Models
{
    public class Logger
    {
        public string LogPath { get; }
        public LogType.LogTypeEnum LogType { get; set; }


        public Logger(string logPath, LogType.LogTypeEnum logType)
        {
            if (string.IsNullOrWhiteSpace(logPath))
                throw new ArgumentException("Log path cannot be null or empty.", nameof(logPath));

            if (!Path.IsPathRooted(logPath)) // Ensure it's an absolute path
                logPath = Path.Combine(AppContext.BaseDirectory, logPath);

            if (!Enum.IsDefined(typeof(LogType.LogTypeEnum), logType))
                throw new ArgumentException("Invalid log type.", nameof(logType));

            LogPath = logPath;
            LogType = logType;
        }
    }
}