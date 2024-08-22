using Fun.SimpleGame.Configurations.Enemy.Enums;

namespace Fun.SimpleGame.Configurations.Enemy.Models
{
    public sealed class SpawnLocation
    {
        public int Offset { get; }

        public SpawnArea? Area { get; }

        public SpawnLocation(int offset)
        {
            Offset = offset;
        }

        public SpawnLocation(SpawnArea area)
        {
            Area = area;
        }
    }
}