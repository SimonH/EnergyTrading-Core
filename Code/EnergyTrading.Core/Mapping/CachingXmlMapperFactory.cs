namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Implementation of <see cref="IXmlMapperFactory" /> that caches results.
    /// </summary>
    public class CachingXmlMapperFactory : DictionaryCachingXmlMapperFactory<ConcurrentDictionary<string, object>> 
    {

        public CachingXmlMapperFactory(IXmlMapperFactory factory) : base(factory)
        {
        }

        protected override ConcurrentDictionary<string, object> CreateDictionary()
        {
            return new ConcurrentDictionary<string, object>();
        }

        /// <contentfrom cref="IXmlMapperFactory.Mapper" />
        public override object Mapper(Type source, Type destination, string name = null)
        {
            var key = Key(source, destination, name);

            return Mappers.GetOrAdd(key, s => Factory.Mapper(source, destination, name));
        }
    }
}