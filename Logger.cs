using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Logger
{
    public class Logger
    {
        private readonly string LogPath;
        public Logger(string logPath)
        {
            LogPath = logPath;
        }
        public void Log(object data)
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }
            string logFilePath = Path.Combine(LogPath, DateTime.Now.ToString("d-MM-yyyy") + ".json");
            List<object> logEntries = new List<object>();
            if (File.Exists(logFilePath))
            {
                string existingContent = File.ReadAllText(logFilePath);
                if (!string.IsNullOrWhiteSpace(existingContent))
                {
                    try
                    {
                        logEntries = JsonSerializer.Deserialize<List<object>>(existingContent) ?? new List<object>();
                    }
                    catch (JsonException)
                    {
                        logEntries = new List<object>();
                    }
                }
            }
            logEntries.Add(data);
            string json = JsonSerializer.Serialize(logEntries, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(logFilePath, json);
        }
    }
}