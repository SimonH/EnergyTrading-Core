using System;
using System.Runtime.Serialization;

namespace EnergyTrading.Contracts.Logging
{
    [Serializable]
    public class LogMessageException : Exception
    {
        protected LogMessageException(string message, LogMessageException innerException) : base(message, innerException)
        {
        }

        protected LogMessageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public new string StackTrace { get; set; }

        public static LogMessageException FromException(Exception exception)
        {
            if (exception == null)
            {
                return null;
            }

            return new LogMessageException(exception.Message, FromException(exception.InnerException))
            {
                Source = exception.Source,
                HResult = exception.HResult,
                StackTrace = exception.StackTrace
            };
        }
    }
}