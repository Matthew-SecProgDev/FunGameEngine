using Fun.Engine.Configurations;
using Fun.Engine;
using Fun.SimpleGame.States.Gameplay;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fun.SimpleGame
{
    public static class Program
    {
        //640x360: Suitable for retro and pixel art games
        //1280x720: Common for standard HD games
        //1920x1080: Full HD resolution for more detailed games
        //2560x1440: For high-end displays with more visual fidelity

        //Considerations:
        //  Aspect Ratio: Common ratios are 16:9 or 4:3.
        //  Target Platform: Consider devices like PCs, consoles, or mobile.
        //  Performance: Higher resolutions require more processing power.
        //  Art Style: Pixel art games often use lower resolutions for a retro feel.

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("MainConfig.json")
                //.AddPlayer()
                //.AddXmlFile("EnemyConfig.xml")
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
            //configuration.GetSection(PlayerOption.ConfigurationSectionKey).
#endregion

#region Services
            services.AddSingleton<MainGame>();
#endregion
        }
    }
}