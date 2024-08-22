using System.Collections.Generic;
using Fun.Engine;
using Fun.Engine.Input;
using Fun.SimpleGame.Entities.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fun.SimpleGame.States.Gameplay
{
    public class GameplayInputMapper : BaseInputMapper
    {
        private float _idleThreshold;
        private bool _isIdling = true;

        public override IEnumerable<BaseInputCommand> GetKeyboardState(GameTime gameTime)
        {
            var commands = new List<GameplayInputCommand>();
            var keyboard = Fun.Engine.Input.Keyboard.Instance;
            var totalTime = gameTime.GetTotalSeconds();

            // Check if the player should transition to idle state
            if (!_isIdling && totalTime > _idleThreshold)
            {
                _isIdling = true;
            }

            if (keyboard.IsKeyDown(Keys.Escape))
            {
                commands.Add(new GameplayInputCommand.GameExit());
            }

            if (keyboard.IsKeyDown(Keys.E))
            {
                commands.Add(new GameplayInputCommand.PlayerPickUps());
            }

            if (keyboard.IsKeyDown(Keys.Y))
            {
                commands.Add(new GameplayInputCommand.PlayerAttack(PlayerAttackType.Attack1));
                SetNextIdleThreshold(totalTime, 0.4f);
            }
            else if (keyboard.IsKeyDown(Keys.G))
            {
                commands.Add(new GameplayInputCommand.PlayerAttack(PlayerAttackType.Attack2));
                SetNextIdleThreshold(totalTime, 0.4f);
            }
            else if (keyboard.IsKeyDown(Keys.H))
            {
                commands.Add(new GameplayInputCommand.PlayerAttack(PlayerAttackType.Attack3));
                SetNextIdleThreshold(totalTime, 0.3f);
            }
            else if (keyboard.IsKeyDown(Keys.J))
            {
                commands.Add(new GameplayInputCommand.PlayerAttack(PlayerAttackType.Shoot));
                SetNextIdleThreshold(totalTime, 0.75f);
            }
            else
            {
                if (keyboard.IsKeyDown(Keys.W))
                {
                    commands.Add(new GameplayInputCommand.PlayerMoveUp());
                    SetNextIdleThreshold(totalTime, 0.15f);
                }

                if (keyboard.IsKeyDown(Keys.S))
                {
                    commands.Add(new GameplayInputCommand.PlayerMoveDown());
                    SetNextIdleThreshold(totalTime, 0.15f);
                }

                if (keyboard.IsKeyDown(Keys.D))
                {
                    commands.Add(new GameplayInputCommand.PlayerMoveRight());
                    SetNextIdleThreshold(totalTime, 0.15f);
                }

                if (keyboard.IsKeyDown(Keys.A))
                {
                    commands.Add(new GameplayInputCommand.PlayerMoveLeft());
                    SetNextIdleThreshold(totalTime, 0.15f);
                }
            }

            // Add idle command if no other commands are being processed
            if (_isIdling)
            {
                commands.Add(new GameplayInputCommand.PlayerStopsMoving());
                SetNextIdleThreshold(totalTime, 0.49f);
            }

            return commands;
        }

        private void SetNextIdleThreshold(float totalTime, float delay)
        {
            _idleThreshold = totalTime + delay;
            _isIdling = false;
        }
    }
}