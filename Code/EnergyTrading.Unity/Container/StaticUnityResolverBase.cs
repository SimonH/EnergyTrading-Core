using EnergyTrading.Container.Unity;
using Microsoft.Practices.Unity;

namespace EnergyTrading.Container
{
    public abstract class StaticUnityResolverBase<TDerivedResolver> where TDerivedResolver : StaticUnityResolverBase<TDerivedResolver>, new()
    {
        private static IUnityContainer _container;
        private static IUnityContainer Container => _container ?? (_container = InitialiseContainer());

        private static IUnityContainer InitialiseContainer()
        {
            var ret = new UnityContainer();
            ret.StandardConfiguration();
            new TDerivedResolver().RegisterItems(ret);
            return ret;
        }

        public static T Resolve<T>(string name = null)
        {
            return string.IsNullOrEmpty(name) ? Container.Resolve<T>() : Container.Resolve<T>(name);
        }

        protected abstract void RegisterItems(IUnityContainer container);
    }
}