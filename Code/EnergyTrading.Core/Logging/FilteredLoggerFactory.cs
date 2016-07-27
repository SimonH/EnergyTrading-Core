using System;

namespace EnergyTrading.Logging
{
    public class FilteredLoggerFactory : WrappingLoggerFactory
    {
        private readonly ILoggerFactory wrappedFactory;
        private readonly LogFilterLevel filterLevel;
        public FilteredLoggerFactory(ILoggerFactory factoryToWrap, LogFilterLevel level) : base(factoryToWrap, logger => new FilteredLogger(logger, level))
        {
        }
    }
}