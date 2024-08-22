namespace Fun.SimpleGame.Configurations.Game
{
    public sealed class NameValuePair
    {
        public required string Name { get; set; }

        public required string Value { get; set; }
    }

    public class CharacterOption
    {
        public required string Path { get; set; }

        public required NameValuePair[] States { get; set; }
    }

    public sealed class PlayerOption : CharacterOption
    {
        public const string ConfigurationSectionKey = "assets:textures:player";

        public required NameValuePair[] Weapons { get; set; }
    }

    public sealed class NameValuesPair
    {
        public required string Name { get; set; }

        public required string[] Values { get; set; }
    }

    public sealed class NamedValueCollection
    {
        public required string Name { get; set; }

        public required NameValuePair[] Values { get; set; }
    }

    public sealed class AudioOption
    {
        public NameValuePair[] Sounds { get; set; }

        public NameValuePair[] Music { get; set; }
    }

    public sealed class SettingOption
    {
        public float AudioMasterVolume { get; set; }

        public float AudioMusicVolume { get; set; }

        public float AudioSfxVolume { get; set; }
    }

    public sealed class LevelOption
    {
        public required string Name { get; set; }

        public required string Background { get; set; }

        public int PlayerHealth { get; set; }

        public float PlayerSpeed { get; set; }

        public required string SpawnEnemy { get; set; }
    }

    public class GameConfig
    {
        public required PlayerOption Player { get; set; }

        public required CharacterOption[] Enemies { get; set; }

        public required string Splash { get; set; }

        public required NameValuesPair[] Effects { get; set; }

        public required NamedValueCollection[] UI { get; set; }

        public required AudioOption[] Audio { get; set; }

        public required NameValuePair[] Fonts { get; set; }

        public required SettingOption Setting { get; set; }

        public required LevelOption[] Levels { get; set; }

        public required NameValuePair[] Controls { get; set; }
    }
}