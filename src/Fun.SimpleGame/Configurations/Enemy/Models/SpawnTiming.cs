namespace Fun.SimpleGame.Configurations.Enemy.Models
{
    public sealed class SpawnTiming
    {
        public int TriggerPoint { get; }

        public bool StartImmediately { get; } = false;

        public SpawnTiming(int triggerPoint)
        {
            TriggerPoint = triggerPoint;
        }

        public SpawnTiming(bool startImmediately)
        {
            StartImmediately = startImmediately;
        }
    }
}