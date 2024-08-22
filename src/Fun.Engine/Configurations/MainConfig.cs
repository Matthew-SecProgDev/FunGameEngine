namespace Fun.Engine.Configurations
{
    public sealed class MainConfig
    {
        public int Width { get; set; } = 1280;

        public int Height { get; set; } = 720;

        public required string RootDirectory { get; set; }

        public bool IsMouseVisible { get; set; }

        public bool IsFullScreen { get; set; }

        public bool IsFixedTimeStep { get; set; }

        public int UpdatesCountPerSecond { get; set; }
    }
}