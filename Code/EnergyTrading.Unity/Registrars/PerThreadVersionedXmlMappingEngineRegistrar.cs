using System;
using System.Collections.Generic;
using EnergyTrading.Mapping;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace EnergyTrading.Registrars
{
    public abstract class PerThreadVersionedXmlMappingEngineRegistrar : VersionedXmlMappingEngineRegistrar
    {
        protected override IUnityContainer RegisterVersionedEngine(IUnityContainer container, Version version, IEnumerable<MapperArea> areas)
        {
            var versioned = RegisterMappers(areas);

            var locator = versioned.Resolve<IServiceLocator>();

            if (CacheMappers)
            {
                versioned.RegisterType<IXmlMapperFactory, LocatorXmlMapperFactory>("locatorMapperFactory", new PerThreadLifetimeManager(), new InjectionConstructor(new ResolvedParameter<IServiceLocator>()));
                container.RegisterType<IXmlMapperFactory, LocatorXmlMapperFactory>(ToVersionString(version) + "locatorMapperFactory", new PerThreadLifetimeManager(), new InjectionConstructor(locator));
                versioned.RegisterType<IXmlMapperFactory, PerThreadCachingXmlMapperFactory>(new PerThreadLifetimeManager(), new InjectionConstructor(new ResolvedParameter<IXmlMapperFactory>("locatorMapperFactory")));
                container.RegisterType<IXmlMapperFactory, PerThreadCachingXmlMapperFactory>(ToVersionString(version), new PerThreadLifetimeManager(), new InjectionConstructor(new ResolvedParameter<IXmlMapperFactory>(ToVersionString(version) + "locatorMapperFactory")));
            }
            else
            {
                versioned.RegisterType<IXmlMapperFactory, LocatorXmlMapperFactory>(new PerThreadLifetimeManager(), new InjectionConstructor(new ResolvedParameter<IServiceLocator>()));
                container.RegisterType<IXmlMapperFactory, LocatorXmlMapperFactory>(ToVersionString(version), new PerThreadLifetimeManager(), new InjectionConstructor(new ResolvedParameter<IServiceLocator>()));
            }
            versioned.RegisterType<IXmlMappingEngine, PerThreadXmlMappingEngine>(new PerThreadLifetimeManager(), new InjectionConstructor(new ResolvedParameter<IXmlMapperFactory>()));
            container.RegisterType<IXmlMappingEngine, PerThreadXmlMappingEngine>(ToVersionString(version), new PerThreadLifetimeManager(), new InjectionConstructor(new ResolvedParameter<IXmlMapperFactory>(ToVersionString(version))));

            // Ok, record the schema as one we are interested in
            if (!string.IsNullOrEmpty(SchemaName))
            {
                SchemaRegistry(container).RegisterSchema(SchemaName);
            }

            RegisterSchemaSetVersion(container, version);

            return versioned;
        }
    }
}