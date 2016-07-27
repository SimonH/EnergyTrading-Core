using System;

namespace EnergyTrading.Logging
{
    public class WrappingLoggerFactory : ILoggerFactory
    {
        private readonly ILoggerFactory wrappedFactory;
        private readonly Func<ILogger, ILogger> wrapLoggerFunc;
        public WrappingLoggerFactory(ILoggerFactory wrappedFactory, Func<ILogger, ILogger> wrapLoggerFunc)
        {
            if (wrapLoggerFunc == null)
            {
                throw new ArgumentNullException(nameof(wrapLoggerFunc));
            }
            if (wrappedFactory == null)
            {
                throw new ArgumentNullException(nameof(wrappedFactory));
            }
            this.wrappedFactory = wrappedFactory;
            this.wrapLoggerFunc = wrapLoggerFunc;
        }

        private ILogger Wrap(ILogger logger)
        {
            return logger == null ? null : wrapLoggerFunc(logger);
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