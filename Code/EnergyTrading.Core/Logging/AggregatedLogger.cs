using System;
using System.Collections.Generic;
using System.Linq;

namespace EnergyTrading.Logging
{
    public class AggregatedLogger : ILogger
    {
        private readonly List<ILogger> loggers = new List<ILogger>();
        public AggregatedLogger(params ILogger[] loggers)
        {
            foreach (var logger in loggers)
            {
                this.loggers.Add(logger);
            }
        }

        public AggregatedLogger(IEnumerable<ILogger> loggers) : this(loggers.ToArray())
        {
        }

        public bool IsDebugEnabled => loggers.Any(l => l.IsDebugEnabled);
        public bool IsInfoEnabled => loggers.Any(l => l.IsInfoEnabled);
        public bool IsWarnEnabled => loggers.Any(l => l.IsWarnEnabled);
        public bool IsErrorEnabled => loggers.Any(l => l.IsErrorEnabled);
        public bool IsFatalEnabled => loggers.Any(l => l.IsFatalEnabled);

        private void Loop(Action<ILogger> loggerAction, Func<ILogger, bool> testFunc)
        {
            loggers.ForEach(l =>
            {
                if (testFunc(l))
                {
                    loggerAction(l);
                }
            });
        }

        public void Debug(string message)
        {
            Loop(l => l.Debug(message), l => l.IsDebugEnabled);
        }

        public void Debug(string message, Exception exception)
        {
            Loop(l => l.Debug(message, exception), l => l.IsDebugEnabled);
        }

        public void DebugFormat(string format, params object[] parameters)
        {
            Loop(l => l.DebugFormat(format, parameters), l => l.IsDebugEnabled);
        }

        public void Info(string message)
        {
            Loop(l => l.Info(message), l => l.IsInfoEnabled);
        }

        public void Info(string message, Exception exception)
        {
            Loop(l => l.Info(message, exception), l => l.IsInfoEnabled);
        }

        public void InfoFormat(string format, params object[] parameters)
        {
            Loop(l => l.InfoFormat(format, parameters), l => l.IsInfoEnabled);
        }

        public void Warn(string message)
        {
            Loop(l => l.Warn(message), l => l.IsWarnEnabled);
        }

        public void Warn(string message, Exception exception)
        {
            Loop(l => l.Warn(message, exception), l => l.IsWarnEnabled);
        }

        public void WarnFormat(string format, params object[] parameters)
        {
            Loop(l => l.WarnFormat(format, parameters), l => l.IsWarnEnabled);
        }

        public void Error(string message)
        {
            Loop(l => l.Error(message), l => l.IsErrorEnabled);
        }

        public void Error(string message, Exception exception)
        {
            Loop(l => l.Error(message, exception), l => l.IsErrorEnabled);
        }

        public void ErrorFormat(string format, params object[] parameters)
        {
            Loop(l => l.ErrorFormat(format, parameters), l => l.IsErrorEnabled);
        }

        public void Fatal(string message)
        {
            Loop(l => l.Fatal(message), l => l.IsFatalEnabled);
        }

        public void Fatal(string message, Exception exception)
        {
            Loop(l => l.Fatal(message, exception), l => l.IsFatalEnabled);
        }

        public void FatalFormat(string format, params object[] parameters)
        {
            Loop(l => l.FatalFormat(format, parameters), l => l.IsFatalEnabled);
        }
    }
}