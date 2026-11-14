using Fun.Engine;
using Fun.Engine.Configurations;
using Fun.SimpleGame.States.Gameplay;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Fun.SimpleGame
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("MainConfig.json", optional: false)
                .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            using var mainGame = serviceProvider.GetRequiredService<MainGame>();
            mainGame.SetCurrentGameState(new GameplayState());
            mainGame.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
#region Configure
            services.Configure<MainConfig>(i => configuration.GetSection(nameof(MainConfig)).Bind(i));
#endregion

#region Services
            services.AddSingleton<MainGame>();
#endregion
        }
    }
}