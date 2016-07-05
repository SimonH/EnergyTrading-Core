using System;
using System.Collections.Generic;
using System.Linq;

namespace EnergyTrading.Logging
{
    public class AggregateLoggerFactory : ILoggerFactory
    {
        private readonly List<ILoggerFactory> factories = new List<ILoggerFactory>();
        public AggregateLoggerFactory(params ILoggerFactory[] factories)
        {
            foreach (var factory in factories)
            {
                this.factories.Add(factory);
            }
        }

        public AggregateLoggerFactory(IEnumerable<ILoggerFactory> factories) : this(factories.ToArray())
        {
        }

        private ILogger Wrap(Func<ILoggerFactory, ILogger> loggerFunc)
        {
            return new AggregatedLogger(factories.Select(loggerFunc));
        }

        public ILogger GetLogger(string name)
        {
            return Wrap(lf => lf.GetLogger(name));
        }

        public ILogger GetLogger<T>()
        {
            return Wrap(lf => lf.GetLogger<T>());
        }

        public ILogger GetLogger(Type type)
        {
            return Wrap(lf => lf.GetLogger(type));
        }

        public void Initialize()
        {
            factories.ForEach(lf => lf.Initialize());
        }

        public void Shutdown()
        {
            factories.ForEach(lf => lf.Shutdown());
        }
    }
}