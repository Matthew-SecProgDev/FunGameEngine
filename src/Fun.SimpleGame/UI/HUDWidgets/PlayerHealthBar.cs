using Fun.Engine.Events;
using Fun.Engine.UI;
using Fun.SimpleGame.States.Gameplay;
using Fun.SimpleGame.UI.HUDWidgets.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.SimpleGame.UI.HUDWidgets
{
    public class PlayerHealthBar : BaseUIObject
    {
        private readonly Texture2D _fillTexture;
        private readonly int _health;

        private float _percentage = 1f;

        public PlayerHealthBar(Vector2 size, HealthBarOption option, int health, Vector2 position) : base(size, position)
        {
            Texture = option.BackgroundTexture;
            _fillTexture = option.FillTexture;
            _health = health;
        }

        public override void OnNotify(BaseGameStateEvent @event)
        {
            if (@event is GameplayEvents.PlayerHit hitEvent)
            {
                if (hitEvent.Value >= _health)
                {
                    _percentage = 0f;
                }
                else
                {
                    var damagePercentage = (float)hitEvent.Value / _health;
                    _percentage = FunMath.Clamp(_percentage - damagePercentage, 0f, 1f);
                }
            }
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            var positionX = (int)Position.X;
            var positionY = (int)Position.Y;
            var sizeY = (int)Size.Y;

            var backgroundRect = new Rectangle(positionX, positionY, (int)Size.X, sizeY);
            sprites.Draw(Texture, backgroundRect, null, Color.White);

            var fillRect = new Rectangle(positionX, positionY, (int)(Size.X * _percentage), sizeY);
            sprites.Draw(_fillTexture, fillRect, null, GetFillColor(_percentage));
        }

        private static Color GetFillColor(float percentage)
        {
            if (percentage >= 0.9f)
                return Color.Green; //it's temporarily set

            if (percentage is >= 0.8f and < 0.9f)
                return Color.GreenYellow;

            if (percentage is >= 0.6f and < 0.8f)
                return Color.LightYellow;

            if (percentage is >= 0.4f and < 0.6f)
                return Color.Orange;

            return Color.Red;
        }
    }
}