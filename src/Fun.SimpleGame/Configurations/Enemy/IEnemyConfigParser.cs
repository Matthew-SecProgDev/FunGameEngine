using Fun.SimpleGame.Configurations.Enemy.Models;

namespace Fun.SimpleGame.Configurations.Enemy
{
    public interface IEnemyConfigParser
    {
        public EnemyConfig Parse(string fileName);
    }
}