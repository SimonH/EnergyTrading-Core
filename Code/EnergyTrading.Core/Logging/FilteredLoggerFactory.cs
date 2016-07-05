using System;

namespace EnergyTrading.Logging
{
    public class FilteredLoggerFactory : ILoggerFactory
    {
        private readonly ILoggerFactory wrappedFactory;
        private readonly LogFilterLevel filterLevel;
        public FilteredLoggerFactory(ILoggerFactory factoryToWrap, LogFilterLevel level)
        {
            if (factoryToWrap == null)
            {
                throw new ArgumentNullException(nameof(factoryToWrap));
            }
            wrappedFactory = factoryToWrap;
            filterLevel = level;
        }

        private ILogger Wrap(ILogger logger)
        {
            if (logger == null)
            {
                return null;
            }
            return new FilteredLogger(logger, filterLevel);
        }

        public ILogger GetLogger(string name)
        {
            return Wrap(wrappedFactory.GetLogger(name));
        }

        public ILogger GetLogger<T>()
        {
            return Wrap(wrappedFactory.GetLogger<T>());
        }

        public ILogger GetLogger(Type type)
        {
            return Wrap(wrappedFactory.GetLogger(type));
        }

        public void Initialize()
        {
            wrappedFactory.Initialize();
        }

        public void Shutdown()
        {
            wrappedFactory.Shutdown();
        }
    }
}