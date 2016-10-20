using System;
using System.Collections.Generic;

namespace EnergyTrading.Mapping
{
    public abstract class DictionaryCachingXmlMapperFactory<TDictionary> : IXmlMapperFactory where TDictionary : class, IDictionary<string, object>
    {
        private readonly IXmlMapperFactory factory;
        private TDictionary mappers;

        protected abstract TDictionary CreateDictionary();

        protected TDictionary Mappers => mappers ?? (mappers = CreateDictionary());
        protected IXmlMapperFactory Factory => factory;

        public DictionaryCachingXmlMapperFactory(IXmlMapperFactory factory)
        {
            this.factory = factory;
        }

        /// <contentfrom cref="IXmlMapperFactory.Mapper{T, U}" />
        public IXmlMapper<TSource, TDestination> Mapper<TSource, TDestination>(string name = null)
        {
            return (IXmlMapper<TSource, TDestination>)Mapper(typeof(TSource), typeof(TDestination), name);
        }

        /// <contentfrom cref="IXmlMapperFactory.Mapper" />
        public abstract object Mapper(Type source, Type destination, string name = null);

        /// <contentfrom cref="IXmlMapperFactory.Register{T, U}" />
        public void Register<TSource, TDestination>(IXmlMapper<TSource, TDestination> mapper, string name = null)
        {
            if (mapper == null) { throw new ArgumentNullException("mapper"); }

            var key = Key(typeof(TSource), typeof(TDestination), name);

            Mappers[key] = mapper;
        }

        protected static string Key(Type source, Type destination, string name = null)
        {
            return source.FullName + "|" + destination.FullName + "|" + name;
        }
    }
}