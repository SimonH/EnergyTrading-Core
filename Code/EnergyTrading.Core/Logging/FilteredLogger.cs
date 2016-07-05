using System;

namespace EnergyTrading.Logging
{
    public class FilteredLogger : ILogger
    {
        private readonly ILogger wrappedLogger;
        private readonly LogFilterLevel logFilterLevel;
        public FilteredLogger(ILogger loggerToWrap, LogFilterLevel filterLevel)
        {
            if (loggerToWrap == null)
            {
                throw new ArgumentNullException(nameof(loggerToWrap));
            }
            wrappedLogger = loggerToWrap;
            logFilterLevel = filterLevel;
        }

        public bool IsDebugEnabled => (logFilterLevel & LogFilterLevel.Debug) != LogFilterLevel.Debug;
        public bool IsInfoEnabled => (logFilterLevel & LogFilterLevel.Info) != LogFilterLevel.Info;
        public bool IsWarnEnabled => (logFilterLevel & LogFilterLevel.Warn) != LogFilterLevel.Warn;
        public bool IsErrorEnabled => (logFilterLevel & LogFilterLevel.Error) != LogFilterLevel.Error;
        public bool IsFatalEnabled => (logFilterLevel & LogFilterLevel.Fatal) != LogFilterLevel.Fatal;
        public void Debug(string message)
        {
            if (IsDebugEnabled)
            {
                wrappedLogger.Debug(message);
            }
        }

        public void Debug(string message, Exception exception)
        {
            if (IsDebugEnabled)
            {
                wrappedLogger.Debug(message, exception);
            }
        }

        public void DebugFormat(string format, params object[] parameters)
        {
            if (IsDebugEnabled)
            {
                wrappedLogger.DebugFormat(format, parameters);
            }
        }

        public void Info(string message)
        {
            if (IsInfoEnabled)
            {
                wrappedLogger.Info(message);
            }
        }

        public void Info(string message, Exception exception)
        {
            if (IsInfoEnabled)
            {
                wrappedLogger.Info(message, exception);
            }
        }

        public void InfoFormat(string format, params object[] parameters)
        {
            if (IsInfoEnabled)
            {
                wrappedLogger.InfoFormat(format, parameters);
            }

        }

        public void Warn(string message)
        {
            if (IsWarnEnabled)
            {
                wrappedLogger.Warn(message);
            }
        }

        public void Warn(string message, Exception exception)
        {
            if (IsWarnEnabled)
            {
                wrappedLogger.Warn(message, exception);
            }
        }

        public void WarnFormat(string format, params object[] parameters)
        {
            if (IsWarnEnabled)
            {
                wrappedLogger.WarnFormat(format, parameters);
            }
        }

        public void Error(string message)
        {
            if (IsErrorEnabled)
            {
                wrappedLogger.Error(message);
            }
        }

        public void Error(string message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                wrappedLogger.Error(message, exception);
            }
        }

        public void ErrorFormat(string format, params object[] parameters)
        {
            if (IsErrorEnabled)
            {
                wrappedLogger.ErrorFormat(format, parameters);
            }
        }

        public void Fatal(string message)
        {
            if (IsFatalEnabled)
            {
                wrappedLogger.Fatal(message);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            if (IsFatalEnabled)
            {
                wrappedLogger.Fatal(message, exception);
            }
        }

        public void FatalFormat(string format, params object[] parameters)
        {
            if (IsFatalEnabled)
            {
                wrappedLogger.FatalFormat(format, parameters);
            }
        }
    }
}