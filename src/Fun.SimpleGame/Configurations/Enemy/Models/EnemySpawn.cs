using Fun.SimpleGame.Configurations.Enemy.Enums;

namespace Fun.SimpleGame.Configurations.Enemy.Models
{
    public sealed class EnemySpawn
    {
        public EnemyRole Role { get; init; }

        public int Health { get; init; }

        public required SpawnLocation Position { get; init; }

        public required SpawnTiming BuildTime { get; init; }

        public bool Active { get; init; }
    }
}