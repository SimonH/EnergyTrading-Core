using EnergyTrading.Configuration;
using EnergyTrading.Container;
using EnergyTrading.Registrars;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace EnergyTrading.UnitTest.Container
{
    [TestFixture]
    public class StaticUnityResolverFixture
    {
        public class ConfigManagerUnityResolver : StaticUnityResolverBase<ConfigManagerUnityResolver>
        {
            protected override void RegisterItems(IUnityContainer container)
            {
                new ConfigurationManagerRegistrar().Register(container);
            }
        }

        [Test]
        public void CorrectlyResolvesRegisteredItems()
        {
            Assert.That(ConfigManagerUnityResolver.Resolve<IConfigurationManager>(), Is.Not.Null.And.TypeOf<AppConfigConfigurationManager>());
        }
    }
}