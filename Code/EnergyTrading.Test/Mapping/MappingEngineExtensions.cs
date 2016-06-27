namespace EnergyTrading.Test.Mapping
{
    using System;

    using EnergyTrading.Mapping;

    using NUnit.Framework;

    /// <summary>
    /// Helper methods for testing mapping engines.
    /// </summary>
    public static class MappingEngineExtensions
    {
        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <typeparam name="TMapper">Type of mapper we expect</typeparam>        
        /// <param name="engine">Engine to use.</param>        
        /// <param name="name"></param>
        public static void ResolveMapper<TSource, TDestination, TMapper>(this IMappingEngine engine, string name = null)
        {
            var simpleEngine = engine as SimpleMappingEngine;

            simpleEngine.ResolveMapper<TSource, TDestination>(typeof(TMapper), name);
        }

        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <param name="engine">Engine to use.</param>        
        /// <param name="implementation">Type of mapper we expect</param>
        /// <param name="name"></param>
        public static void  ResolveMapper<TSource, TDestination>(this IMappingEngine engine, Type implementation, string name = null)
        {
            var simpleEngine = engine as SimpleMappingEngine;

            simpleEngine.ResolveMapper<TSource, TDestination>(implementation, name);
        }

        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <param name="engine">Engine to use.</param>        
        /// <param name="implementation">Type of mapper we expect</param>
        /// <param name="name"></param>
        public static void ResolveMapper<TSource, TDestination>(this SimpleMappingEngine engine, Type implementation, string name = null)
        {
            if (engine == null)
            {
                throw new NotSupportedException("Must supply engine");
            }
            IMapper<TSource, TDestination> mapper;
            if (name == null)
            {
                mapper = engine.Mapper<TSource, TDestination>();
            }
            else
            {
                mapper = engine.Mapper<TSource, TDestination>(name);
            }
            Assert.AreSame(mapper.GetType(), implementation, string.Format("{0} vs {1}", implementation.FullName, mapper.GetType().FullName));
        }

        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <typeparam name="TMapper">Type of mapper we expect</typeparam>
        /// <param name="engine">Engine to use.</param>  
        /// <param name="name"></param>
        public static void ResolveMapper<TSource, TDestination, TMapper>(this IXmlMappingEngine engine, string name = null)
        {
            var xmlEngine = engine as XmlMappingEngine;

            xmlEngine.ResolveMapper<TSource, TDestination>(typeof(TMapper), name);
        }

        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <param name="engine">Engine to use.</param>  
        /// <param name="implementation">Type of mapper we expect</param>
        /// <param name="name"></param>
        public static void ResolveMapper<TSource, TDestination>(this IXmlMappingEngine engine, Type implementation, string name = null)
        {
            var xmlEngine = engine as XmlMappingEngine;

            xmlEngine.ResolveMapper<TSource, TDestination>(implementation, name);
        }

        /// <summary>
        /// Check whether a mapper is in a version.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Result type</typeparam>
        /// <param name="engine">Engine to use.</param>  
        /// <param name="implementation">Type of mapper we expect</param>
        /// <param name="name"></param>
        public static void ResolveMapper<TSource, TDestination>(this XmlMappingEngine engine, Type implementation, string name = null)
        {
            if (engine == null)
            {
                throw new NotSupportedException("Must supply engine");
            }
            var mapper = engine.Mapper<TSource, TDestination>(name);            
            Assert.AreSame(mapper.GetType(), implementation, string.Format("{0} vs {1}", implementation.FullName, mapper.GetType().FullName));
        }
    }
}