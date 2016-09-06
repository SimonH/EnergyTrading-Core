using System;

namespace EnergyTrading.Logging
{
    public abstract class WrapLoggingActionsBase : ILogger
    {
        private readonly ILogger loggerToWrap;

        protected ILogger WrappedLogger => loggerToWrap;
        protected WrapLoggingActionsBase(ILogger loggerToWrap)
        {
            if (loggerToWrap == null)
            {
                throw new ArgumentNullException(nameof(loggerToWrap));
            }
            this.loggerToWrap = loggerToWrap;
        }

        public bool IsDebugEnabled => loggerToWrap.IsDebugEnabled;
        public bool IsInfoEnabled => loggerToWrap.IsInfoEnabled;
        public bool IsWarnEnabled => loggerToWrap.IsWarnEnabled;
        public bool IsErrorEnabled => loggerToWrap.IsErrorEnabled;
        public bool IsFatalEnabled => loggerToWrap.IsFatalEnabled;

        protected abstract void WrapLoggingAction(Action<ILogger> logAction);


        public void Debug(string message)
        {
            WrapLoggingAction(logger => logger.Debug(message));
        }

        public void Debug(string message, Exception exception)
        {
            WrapLoggingAction(logger => logger.Debug(message, exception));
        }

        public void DebugFormat(string format, params object[] parameters)
        {
            WrapLoggingAction(logger => logger.DebugFormat(format, parameters));
        }

        public void Info(string message)
        {
            WrapLoggingAction(logger => logger.Info(message));
        }

        public void Info(string message, Exception exception)
        {
            WrapLoggingAction(logger => logger.Info(message, exception));
        }

        public void InfoFormat(string format, params object[] parameters)
        {
            WrapLoggingAction(logger => logger.InfoFormat(format, parameters));
        }

        public void Warn(string message)
        {
            WrapLoggingAction(logger => logger.Warn(message));
        }

        public void Warn(string message, Exception exception)
        {
            WrapLoggingAction(logger => logger.Warn(message, exception));
        }

        public void WarnFormat(string format, params object[] parameters)
        {
            WrapLoggingAction(logger => logger.WarnFormat(format, parameters));
        }

        public void Error(string message)
        {
            WrapLoggingAction(logger => logger.Error(message));
        }

        public void Error(string message, Exception exception)
        {
            WrapLoggingAction(logger => logger.Error(message, exception));
        }

        public void ErrorFormat(string format, params object[] parameters)
        {
            WrapLoggingAction(logger => logger.ErrorFormat(format, parameters));
        }

        public void Fatal(string message)
        {
            WrapLoggingAction(logger => logger.Fatal(message));
        }

        public void Fatal(string message, Exception exception)
        {
            WrapLoggingAction(logger => logger.Fatal(message, exception));
        }

        public void FatalFormat(string format, params object[] parameters)
        {
            WrapLoggingAction(logger => logger.FatalFormat(format, parameters));
        }
    }
}