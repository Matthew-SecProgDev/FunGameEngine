namespace Fun.SimpleGame.Configurations.Game
{
    public interface IGameConfigParser
    {
        public GameConfig Parse(string fileName);
    }
}