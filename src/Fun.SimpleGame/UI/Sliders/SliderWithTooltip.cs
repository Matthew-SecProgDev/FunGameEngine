using Fun.Engine.UI.Sliders;
using Fun.Engine.UI.Tooltips;
using Fun.SimpleGame.UI.Sliders.Models;
using Microsoft.Xna.Framework;

namespace Fun.SimpleGame.UI.Sliders
{
    public class SliderWithTooltip(Vector2 size, SliderWithTooltipOption option, Vector2 position)
        : Slider(size, option, position)
    {
        private readonly BaseTooltip _tooltip = option.Tooltip;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _tooltip.UpdateText(((int)Value).ToString());

            var x = HandleBounds.X - _tooltip.TooltipBounds.Width / 2f + HandleBounds.Width / 2f;
            var y = HandleBounds.Y - _tooltip.TooltipBounds.Height - 10;
            _tooltip.UpdateTooltipPosition((int)x, y);

            var textPosX = (int)(x + _tooltip.TooltipBounds.Width / 2f);
            var textPosY = y + _tooltip.TooltipBounds.Height / 2;
            _tooltip.UpdateTextPosition(textPosX, textPosY);
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            base.Render(sprites);

            _tooltip.Render(sprites);
        }
    }
}