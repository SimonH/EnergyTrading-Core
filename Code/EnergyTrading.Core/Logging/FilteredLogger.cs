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

        private void Call(Action<ILogger> loggerAction, Func<bool> testFunc)
        {
            if (testFunc())
            {
                loggerAction(wrappedLogger);
            }
        }

        public void Debug(string message)
        {
            Call(l => l.Debug(message), () => IsDebugEnabled);
        }

        public void Debug(string message, Exception exception)
        {
            Call(l => l.Debug(message, exception), () => IsDebugEnabled);
        }

        public void DebugFormat(string format, params object[] parameters)
        {
            Call(l => l.DebugFormat(format, parameters), () => IsDebugEnabled);
        }

        public void Info(string message)
        {
            Call(l => l.Info(message), () => IsInfoEnabled);
        }

        public void Info(string message, Exception exception)
        {
            Call(l => l.Info(message, exception), () => IsInfoEnabled);
        }

        public void InfoFormat(string format, params object[] parameters)
        {
            Call(l => l.InfoFormat(format, parameters), () => IsInfoEnabled);
        }

        public void Warn(string message)
        {
            Call(l => l.Warn(message), () => IsWarnEnabled);
        }

        public void Warn(string message, Exception exception)
        {
            Call(l => l.Warn(message, exception), () => IsWarnEnabled);
        }

        public void WarnFormat(string format, params object[] parameters)
        {
            Call(l => l.WarnFormat(format, parameters), () => IsWarnEnabled);
        }

        public void Error(string message)
        {
            Call(l => l.Error(message), () => IsErrorEnabled);
        }

        public void Error(string message, Exception exception)
        {
            Call(l => l.Error(message, exception), () => IsErrorEnabled);
        }

        public void ErrorFormat(string format, params object[] parameters)
        {
            Call(l => l.ErrorFormat(format, parameters), () => IsErrorEnabled);
        }

        public void Fatal(string message)
        {
            Call(l => l.Fatal(message), () => IsFatalEnabled);
        }

        public void Fatal(string message, Exception exception)
        {
            Call(l => l.Fatal(message, exception), () => IsFatalEnabled);
        }

        public void FatalFormat(string format, params object[] parameters)
        {
            Call(l => l.FatalFormat(format, parameters), () => IsFatalEnabled);
        }
    }
}