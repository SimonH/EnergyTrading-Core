﻿namespace EnergyTrading.Registrars
{
    using EnergyTrading.Container.Unity;
    using EnergyTrading.Mapping;
    using EnergyTrading.Xml;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Register the implementation for <see cref="IXmlSchemaRegistry"/>.
    /// </summary>
    public class XmlSchemaRegistryRegistrar : IContainerRegistrar 
    {
        /// <copydocfrom cref="IContainerRegistrar.Register" />
        public void Register(IUnityContainer container)
        {
            // Check to see if we've already done this.
            var registry = container.TryResolve<IXmlSchemaRegistry>();
            if (registry != null)
            {
                return;
            }

            // Need this as a singleton
            container.RegisterType<IXmlSchemaRegistry, XmlSchemaRegistry>(new ContainerControlledLifetimeManager());
            container.RegisterType<IXmlSchemaValidator, XmlSchemaValidator>(new ContainerControlledLifetimeManager());
        }
    }
}