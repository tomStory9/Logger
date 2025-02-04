using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger.Services;

namespace Logger.ViewModels
{
    class LoggerViewModel
    {
        private LoggerService loggerService;
        public LoggerViewModel(string logPath)
        {
            loggerService = new LoggerService(logPath);
        }

        public void AddLog(object data)
        {
            loggerService.AddLog(data);
        }
    }
}
