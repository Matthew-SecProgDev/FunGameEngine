namespace Fun.Engine.Utilities
{
    public static class RandomNumberGenerator
    {
        private static readonly Random Rnd = new();

        public static int Next() => Rnd.Next();

        public static float NextFloat() => Rnd.NextSingle();

        public static int Next(int max) => Rnd.Next(max);

        public static int NextRandom(int min, int max) => Rnd.Next(min, max);

        public static float NextRandom(float max) => (float)Rnd.NextDouble() * max;

        public static float NextRandom(float min, float max) => (float)Rnd.NextDouble() * (max - min) + min;
    }
}