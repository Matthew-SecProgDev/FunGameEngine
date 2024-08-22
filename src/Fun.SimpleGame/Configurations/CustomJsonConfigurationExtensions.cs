using Fun.SimpleGame.Configurations.Game;
using Microsoft.Extensions.Configuration;

namespace Fun.SimpleGame.Configurations
{
    public static class CustomJsonConfigurationExtensions
    {
        public static IConfigurationBuilder AddPlayer(this IConfigurationBuilder builder, string json)
        {
            var source = new PlayerConfigurationSource
            {
                Json = json
            };
            builder.Add(source);
            return builder;
        }
    }
}