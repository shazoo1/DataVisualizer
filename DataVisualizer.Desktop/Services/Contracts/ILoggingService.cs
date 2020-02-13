using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.Services.Contracts
{
    public interface ILoggingService
    {
        void LogError(Exception e, string message, string className, string methodName);
        void LogWarning(string message, string className, string methodName);
        void LogDebug(string message, string className, string methodName);
        void LogInfo(string message, string className, string methodName);
    }
}
