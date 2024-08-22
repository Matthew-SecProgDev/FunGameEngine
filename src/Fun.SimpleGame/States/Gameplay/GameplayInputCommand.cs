using Fun.Engine.Input;
using Fun.SimpleGame.Entities.Enums;

namespace Fun.SimpleGame.States.Gameplay
{
    public class GameplayInputCommand : BaseInputCommand
    {
        public class GameExit : GameplayInputCommand;

        public class PlayerMoveLeft : GameplayInputCommand;

        public class PlayerMoveRight : GameplayInputCommand;

        public class PlayerMoveUp : GameplayInputCommand;

        public class PlayerMoveDown : GameplayInputCommand;

        public class PlayerStopsMoving : GameplayInputCommand;

        public class PlayerAttack(PlayerAttackType type) : GameplayInputCommand
        {
            public PlayerAttackType Type { get; } = type;
        }

        public class PlayerPickUps : GameplayInputCommand;
    }
}