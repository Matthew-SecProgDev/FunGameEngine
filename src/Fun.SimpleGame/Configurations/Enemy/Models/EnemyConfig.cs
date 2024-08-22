using System.Collections.Generic;

namespace Fun.SimpleGame.Configurations.Enemy.Models
{
    public sealed class EnemyConfig
    {
        public required List<EnemySpawn> EnemySpawns { get; init; }

        public required List<EnemyGroup> EnemyGroups { get; init; }
    }
}