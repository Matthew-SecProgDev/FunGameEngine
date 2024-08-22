namespace Fun.SimpleGame.Configurations.Enemy.Models
{
    public sealed class EnemyGroup(EnemySpawn[] enemySpawns)
    {
        public EnemySpawn[] EnemySpawns { get; } = enemySpawns;
    }
}