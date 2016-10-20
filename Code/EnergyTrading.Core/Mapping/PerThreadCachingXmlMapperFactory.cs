using System;
using System.Collections.Generic;

namespace EnergyTrading.Mapping
{
    public class PerThreadCachingXmlMapperFactory : DictionaryCachingXmlMapperFactory<Dictionary<string, object>> 
    {
        public PerThreadCachingXmlMapperFactory(IXmlMapperFactory factory) : base(factory)
        {
        }

        protected override Dictionary<string, object> CreateDictionary()
        {
            return new Dictionary<string, object>();
        }

        /// <contentfrom cref="IXmlMapperFactory.Mapper" />
        public override object Mapper(Type source, Type destination, string name = null)
        {
            var key = Key(source, destination, name);
            if (!Mappers.ContainsKey(key))
            {
                Mappers[key] = Factory.Mapper(source, destination, name);
            }

            return Mappers[key];
        }
    }
}