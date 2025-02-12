using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using LoggerDLL.Models;

namespace LoggerDLL.Services
{
    public class LoggerService
    {
        private readonly Logger _logger;

        public LoggerService(string logPath, LogTypeEnum logType)
        {
            _logger = new Logger(logPath, logType);
        }

        public void AddLog(object data)
        {
            if (!Directory.Exists(_logger.LogPath))
            {
                Directory.CreateDirectory(_logger.LogPath);
            }

            string logFilePath = Path.Combine(_logger.LogPath, DateTime.Now.ToString("d-MM-yyyy"));

            switch (_logger.LogType)
            {
                case LogTypeEnum.Xml:
                    logFilePath += ".xml";
                    SerializeToXml(logFilePath, data);
                    break;
                case LogTypeEnum.Json:
                    logFilePath += ".json";
                    SerializeToJson(logFilePath, data);
                    break;
            }
        }

        private void SerializeToJson(string logFilePath, object data)
        {
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

        private void SerializeToXml(string logFilePath, object data)
        {
            List<object> logEntries = new List<object>();

            if (File.Exists(logFilePath))
            {
                string existingContent = File.ReadAllText(logFilePath);
                if (!string.IsNullOrWhiteSpace(existingContent))
                {
                    try
                    {
                        var serializer = new XmlSerializer(typeof(List<object>));
                        using (var reader = new StringReader(existingContent))
                        {
                            logEntries = (List<object>)serializer.Deserialize(reader);
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        logEntries = new List<object>();
                    }
                }
            }

            logEntries.Add(data);

            var xmlSerializer = new XmlSerializer(typeof(List<object>));
            using (var writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, logEntries);
                File.WriteAllText(logFilePath, writer.ToString());
            }
        }
    }
}
