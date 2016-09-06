using System;
using System.Threading.Tasks;

namespace EnergyTrading.Logging
{
    /// <summary>
    /// Wraps an ILogger instance and uses Task Parallel Libraray to run the call on a background thread
    /// </summary>
    public class BackgroundLogger : WrapLoggingActionsBase
    {
        public BackgroundLogger(ILogger loggerToCall) : base(loggerToCall)
        {
        }

        protected override void WrapLoggingAction(Action<ILogger> logAction)
        {
            new TaskFactory().StartNew(() => logAction(WrappedLogger));
        }
    }
}