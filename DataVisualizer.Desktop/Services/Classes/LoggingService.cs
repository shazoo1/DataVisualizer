using DataVisualizer.Desktop.Services.Contracts;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.Services.Classes
{
    class LoggingService : ILoggingService
    {
        private LoggingConfiguration _config;
        private readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public LoggingService()
        {
            _config = new LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "DataVisualizerLog.log" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            _config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            _config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
       
            NLog.LogManager.Configuration = _config;
        }

        public LoggingService(string filePath)
        {
            _config = new LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = filePath };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            _config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            _config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = _config;
        }
        public void LogDebug(string message, string className, string methodName)
        {
            Logger.Debug(string.Format("{0}.{1}: {2}", className, methodName, message));
        }

        public void LogError(Exception e, string message, string className, string methodName)
        {
            Logger.Error(e, string.Format("{0}.{1}: {2}", className, methodName, message));
        }

        public void LogInfo(string message, string className, string methodName)
        {
            Logger.Trace(string.Format("{0}.{1}: {2}", className, methodName, message));
        }

        public void LogWarning(string message, string className, string methodName)
        {
            Logger.Warn(string.Format("{0}.{1}: WARNING! {2}", className, methodName, message));
        }
    }
}
