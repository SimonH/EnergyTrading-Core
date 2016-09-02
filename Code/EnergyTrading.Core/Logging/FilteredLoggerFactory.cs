using System;

namespace EnergyTrading.Logging
{
    public class FilteredLoggerFactory : WrappingLoggerFactory
    {
        public FilteredLoggerFactory(ILoggerFactory factoryToWrap, LogFilterLevel level) : base(factoryToWrap, logger => new FilteredLogger(logger, level))
        {
        }
    }
}