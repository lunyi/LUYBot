using System.Net.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using SkypeBot.Bots;
using SkypeBot.Config;
using SkypeBot.Service;
using Unity;
using Unity.Injection;

namespace SkypeBot
{
    public sealed class TGPBotFactory : BaseFactory
    {
        public static readonly TGPBotFactory Default = new TGPBotFactory();

        public TGPBotFactory()
        {
            SetUpDefaultImplementations();
        }
        
        private void SetUpDefaultImplementations()
        {
            var container = Container;
            container.RegisterInstance(container);

            container.RegisterType<ITGPBotConfig, TGPBotConfig>(ReuseWithinContainer);

            var config = container.Resolve<ITGPBotConfig>();

            container.RegisterType<INotifier, SkypeNotifier>(ReuseWithinContainer);

            SetupBot(container, config);
        }

        private static void SetupBot(IUnityContainer container, ITGPBotConfig config)
        {
            container.RegisterType<ICredentialProvider, SimpleCredentialProvider>(ReuseWithinContainer,
                new InjectionConstructor(config.AppId, config.AppPassword));
            
            container.RegisterType<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>(ReuseWithinContainer);
            container.RegisterType<IBot, ListenerBot>(ReuseWithinResolve);
        }
    }
}
