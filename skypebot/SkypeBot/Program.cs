using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace SkypeBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new UnityContainerFactory())
                .ConfigureWebHostDefaults(webBuilder => 
                    webBuilder.UseStartup<Startup>());
    }

    public class UnityContainerFactory : IServiceProviderFactory<IUnityContainer>
    {
        private IServiceCollection Services { get; set; }

        public IUnityContainer CreateBuilder(IServiceCollection services)
        {
            Services = services;
            return TGPBotFactory.Default.Container;
        }

        public IServiceProvider CreateServiceProvider(IUnityContainer container)
            => container.BuildServiceProvider(Services);
    }
}
