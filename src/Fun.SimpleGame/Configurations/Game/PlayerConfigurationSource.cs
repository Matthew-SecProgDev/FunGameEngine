using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Primitives;

namespace Fun.SimpleGame.Configurations.Game
{
    public class PlayerConfigurationSource : IConfigurationSource
    {
        public required string Json { get; init; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new PlayerConfigurationProvider(Json);
        }
    }

    public class PlayerConfigurationProvider(string json) : ConfigurationProvider
    {
        public override void Load()
        {
            //var jsonNode = JsonObject.Parse(json);
            //var jsonObject = JObject.Parse(_json);

            // utf8 stream
            var jsonNode = JsonObject.Parse(json);

            Data = ParseJson(jsonNode);
        }

        private static Dictionary<string, string?> ParseJson(JsonNode jObject)
        {

            return new Dictionary<string, string?>();
        }
    }
}