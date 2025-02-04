using System;
using System.IO;

namespace Logger.Models
{
    public class Logger
    {
        public string LogPath { get; }

        public Logger(string logPath)
        {
            if (string.IsNullOrWhiteSpace(logPath))
                throw new ArgumentException("Log path cannot be null or empty.", nameof(logPath));

            if (!Path.IsPathRooted(logPath)) // Ensure it's an absolute path
                logPath = Path.Combine(AppContext.BaseDirectory, logPath);

            LogPath = logPath;
        }
    }
}