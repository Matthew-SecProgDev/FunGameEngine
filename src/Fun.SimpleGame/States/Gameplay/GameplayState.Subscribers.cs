namespace Fun.SimpleGame.States.Gameplay
{
    public partial class GameplayState
    {
        private void InitializePlayerEventSubscribers()
        {
            _eventBus.Subscribe<GameplayEvents.PlayerMoved>(NotifyEvent);

            _eventBus.Subscribe<GameplayEvents.PlayerDirectionChanged>(NotifyEvent);

            _eventBus.Subscribe<GameplayEvents.ScreenShake>(HandleScreenShake);
        }

        private void InitializeTerrainBackgroundEventSubscribers()
        {
            _eventBus.Subscribe<GameplayEvents.ScreenPositionChanged>(NotifyEvent);

            _eventBus.Subscribe<GameplayEvents.PlayerMoveUp>(HandlePlayerMoveUp);

            _eventBus.Subscribe<GameplayEvents.PlayerMoveDown>(HandlePlayerMoveDown);

            _eventBus.Subscribe<GameplayEvents.PlayerMoveLeft>(HandlePlayerMoveLeft);

            _eventBus.Subscribe<GameplayEvents.PlayerMoveRight>(HandlePlayerMoveRight);
        }

        private void UnsubscribeGameplayEvents()
        {
            _eventBus.Unsubscribe<GameplayEvents.PlayerMoved>(NotifyEvent);

            _eventBus.Unsubscribe<GameplayEvents.PlayerDirectionChanged>(NotifyEvent);

            _eventBus.Unsubscribe<GameplayEvents.ScreenShake>(HandleScreenShake);

            _eventBus.Unsubscribe<GameplayEvents.ScreenPositionChanged>(NotifyEvent);

            _eventBus.Unsubscribe<GameplayEvents.PlayerMoveUp>(HandlePlayerMoveUp);

            _eventBus.Unsubscribe<GameplayEvents.PlayerMoveDown>(HandlePlayerMoveDown);

            _eventBus.Unsubscribe<GameplayEvents.PlayerMoveLeft>(HandlePlayerMoveLeft);

            _eventBus.Unsubscribe<GameplayEvents.PlayerMoveRight>(HandlePlayerMoveRight);
        }

        private void HandleScreenShake(GameplayEvents.ScreenShake @event)
        {
            ScreenShake();
        }

        private void HandlePlayerMoveUp(GameplayEvents.PlayerMoveUp @event)
        {
            _player.MoveUp(@event.PosX, @event.PosY);
        }

        private void HandlePlayerMoveDown(GameplayEvents.PlayerMoveDown @event)
        {
            _player.MoveDown(@event.PosX, @event.PosY);
        }

        private void HandlePlayerMoveLeft(GameplayEvents.PlayerMoveLeft @event)
        {
            _player.MoveLeft(@event.PosX, @event.PosY);
        }

        private void HandlePlayerMoveRight(GameplayEvents.PlayerMoveRight @event)
        {
            _player.MoveRight(@event.PosX, @event.PosY);
        }
    }
}