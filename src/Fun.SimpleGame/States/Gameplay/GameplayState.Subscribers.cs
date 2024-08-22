namespace Fun.SimpleGame.States.Gameplay
{
    public partial class GameplayState
    {
        private void InitializePlayerEventSubscribers()
        {
            _eventBus.Subscribe<GameplayEvents.PlayerMoved>(NotifyEvent);//this must be done just for enemies

            _eventBus.Subscribe<GameplayEvents.PlayerDirectionChanged>(NotifyEvent);//this must be done just for enemies

            _eventBus.Subscribe<GameplayEvents.ScreenShake>(_ => ScreenShake());
        }

        private void InitializeTerrainBackgroundEventSubscribers()
        {
            _eventBus.Subscribe<GameplayEvents.ScreenPositionChanged>(NotifyEvent);

            _eventBus.Subscribe<GameplayEvents.PlayerMoveUp>(i => _player.MoveUp(i.PosX, i.PosY));

            _eventBus.Subscribe<GameplayEvents.PlayerMoveDown>(i=> _player.MoveDown(i.PosX, i.PosY));

            _eventBus.Subscribe<GameplayEvents.PlayerMoveLeft>(i => _player.MoveLeft(i.PosX, i.PosY));

            _eventBus.Subscribe<GameplayEvents.PlayerMoveRight>(i => _player.MoveRight(i.PosX, i.PosY));
        }
    }
}