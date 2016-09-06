using System;

namespace EnergyTrading.Logging
{
    public class TryCatchLogger : WrapLoggingActionsBase
    {
        private readonly ILogger exceptionLogger;

        public TryCatchLogger(ILogger loggerToCall, ILogger exceptionLogger) : base(loggerToCall)
        {
            this.exceptionLogger = exceptionLogger;
        }

        protected override void WrapLoggingAction(Action<ILogger> logAction)
        {
            try
            {
                logAction(WrappedLogger);
            }
            catch (Exception exception)
            {
                try
                {
                    if (exceptionLogger != null)
                    {
                        exceptionLogger.Debug("Exception trying to call normal Logger process", exception);
                        logAction(exceptionLogger);
                    }
                }
                catch
                {
                }
            }
        }
    }
}