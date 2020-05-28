using Unity;
using Unity.Lifetime;

namespace SkypeBot.Service
{
    public abstract class BaseFactory
    {
        public IUnityContainer Container { get; } = new UnityContainer();

        protected static ITypeLifetimeManager ReuseWithinContainer => new ContainerControlledLifetimeManager();
        protected static ITypeLifetimeManager ReuseWithinResolve => new PerResolveLifetimeManager();
        protected static ITypeLifetimeManager NewEachTime => new TransientLifetimeManager();
    }
}
