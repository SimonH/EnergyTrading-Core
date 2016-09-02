using System;

namespace EnergyTrading.Logging
{
    public class TryCatchLogger : ILogger
    {
        private readonly ILogger loggerToCall;
        private readonly ILogger exceptionLogger;

        public TryCatchLogger(ILogger loggerToCall, ILogger exceptionLogger)
        {
            if (loggerToCall == null)
            {
                throw new ArgumentNullException(nameof(loggerToCall));
            }
            if (exceptionLogger == null)
            {
                throw new ArgumentNullException(nameof(exceptionLogger));
            }
            this.loggerToCall = loggerToCall;
            this.exceptionLogger = exceptionLogger;
        }


        public bool IsDebugEnabled => loggerToCall.IsDebugEnabled;
        public bool IsInfoEnabled => loggerToCall.IsInfoEnabled;
        public bool IsWarnEnabled => loggerToCall.IsWarnEnabled;
        public bool IsErrorEnabled => loggerToCall.IsErrorEnabled;
        public bool IsFatalEnabled => loggerToCall.IsFatalEnabled;

        private void TryToLog(Action<ILogger> logAction)
        {
            try
            {
                logAction(loggerToCall);
            }
            catch (Exception exception)
            {
                try
                {
                    exceptionLogger.Debug("Exception trying to call normal Logger process", exception);
                    logAction(exceptionLogger);
                }
                catch
                {
                }
            }
        }

        public void Debug(string message)
        {
            TryToLog(logger => logger.Debug(message));
        }

        public void Debug(string message, Exception exception)
        {
            TryToLog(logger => logger.Debug(message, exception));
        }

        public void DebugFormat(string format, params object[] parameters)
        {
            TryToLog(logger => logger.DebugFormat(format, parameters));
        }

        public void Info(string message)
        {
            TryToLog(logger => logger.Info(message));
        }

        public void Info(string message, Exception exception)
        {
            TryToLog(logger => logger.Info(message, exception));
        }

        public void InfoFormat(string format, params object[] parameters)
        {
            TryToLog(logger => logger.InfoFormat(format, parameters));
        }

        public void Warn(string message)
        {
            TryToLog(logger => logger.Warn(message));
        }

        public void Warn(string message, Exception exception)
        {
            TryToLog(logger => logger.Warn(message, exception));
        }

        public void WarnFormat(string format, params object[] parameters)
        {
            TryToLog(logger => logger.WarnFormat(format, parameters));
        }

        public void Error(string message)
        {
            TryToLog(logger => logger.Error(message));
        }

        public void Error(string message, Exception exception)
        {
            TryToLog(logger => logger.Error(message, exception));
        }

        public void ErrorFormat(string format, params object[] parameters)
        {
            TryToLog(logger => logger.ErrorFormat(format, parameters));
        }

        public void Fatal(string message)
        {
            TryToLog(logger => logger.Fatal(message));
        }

        public void Fatal(string message, Exception exception)
        {
            TryToLog(logger => logger.Fatal(message, exception));
        }

        public void FatalFormat(string format, params object[] parameters)
        {
            TryToLog(logger => logger.FatalFormat(format, parameters));
        }
    }
}