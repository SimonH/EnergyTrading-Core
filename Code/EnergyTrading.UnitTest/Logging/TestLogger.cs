using System;
using System.Collections.Generic;
using EnergyTrading.Logging;

namespace EnergyTrading.UnitTest.Logging
{
    public class testLogger : ILogger
    {
        private List<string> messages = new List<string>();
        public IEnumerable<string> Messages => messages;

        private void Log(string message)
        {
            messages.Add(message);
        }

        public bool IsDebugEnabled => true;
        public bool IsInfoEnabled => true;
        public bool IsWarnEnabled => true;
        public bool IsErrorEnabled => true;
        public bool IsFatalEnabled => true;
        public void Debug(string message)
        {
            Log(message);
        }

        public void Debug(string message, Exception exception)
        {
            Log(message + " " + exception.Message);
        }

        public void DebugFormat(string format, params object[] parameters)
        {
            Log(string.Format(format, parameters));
        }

        public void Info(string message)
        {
            Log(message);
        }

        public void Info(string message, Exception exception)
        {
            Log(message + " " + exception.Message);
        }

        public void InfoFormat(string format, params object[] parameters)
        {
            Log(string.Format(format, parameters));
        }

        public void Warn(string message)
        {
            Log(message);
        }

        public void Warn(string message, Exception exception)
        {
            Log(message + " " + exception.Message);
        }

        public void WarnFormat(string format, params object[] parameters)
        {
            Log(string.Format(format, parameters));
        }

        public void Error(string message)
        {
            Log(message);
        }

        public void Error(string message, Exception exception)
        {
            Log(message + " " + exception.Message);
        }

        public void ErrorFormat(string format, params object[] parameters)
        {
            Log(string.Format(format, parameters));
        }

        public void Fatal(string message)
        {
            Log(message);
        }

        public void Fatal(string message, Exception exception)
        {
            Log(message + " " + exception.Message);
        }

        public void FatalFormat(string format, params object[] parameters)
        {
            Log(string.Format(format, parameters));
        }
    }
}