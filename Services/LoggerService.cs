using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LoggerDLL.Models;
namespace LoggerDLL.Services
{
    public class LoggerService
    {

        private readonly Logger _logger;

        public LoggerService(string logPath)
        {
            _logger = new Models.Logger(logPath);
        }

        public void AddLog(object data)
        {
            if (!Directory.Exists(_logger.LogPath))
            {
                Directory.CreateDirectory(_logger.LogPath);
            }

            string logFilePath = Path.Combine(_logger.LogPath, DateTime.Now.ToString("d-MM-yyyy") + ".json");

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
