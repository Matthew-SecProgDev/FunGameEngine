using Fun.Engine.UI.Buttons;
using Fun.Engine.UI.Buttons.Models;
using Fun.Engine.UI.Tooltips;
using Fun.SimpleGame.UI.Tooltips.Models;
using Microsoft.Xna.Framework;

namespace Fun.SimpleGame.UI.Tooltips
{
    public class CloseableTooltip : BaseTooltip
    {
        private readonly Button _button;
        private bool _isVisible = true;

        public CloseableTooltip(Vector2 size, CloseableTooltipOption option, Vector2 position) : base(size, option, position)
        {
            var padding = TooltipBounds.Width * 0.15f;
            var x = TooltipBounds.X + padding;
            var y = TooltipBounds.Y + TooltipBounds.Height / 2f - 10 /*- Font.MeasureString(stringBuilder).Y / 2*/;
            UpdateTextPosition((int)x, (int)y);

            var buttonOption = new TextButtonOption
            {
                NormalTexture = option.ButtonTexture,
                OnClick = () =>
                {
                    _isVisible = false;
                }
            };

            var buttonPosX = TooltipBounds.X + TooltipBounds.Width - option.ButtonPosition.X - 20;
            var buttonPosY = TooltipBounds.Y + option.ButtonPosition.Y;
            _button = new Button(option.ButtonSize, buttonOption, new Vector2(buttonPosX, buttonPosY));
        }

        public override void Update(GameTime gameTime)
        {
            if (!_isVisible)
            {
                return;
            }

            base.Update(gameTime);

            var buttonPosX = TooltipBounds.X + TooltipBounds.Width - 12 - 20;
            var buttonPosY = TooltipBounds.Y + 12;
            _button.UpdateButtonPosition(buttonPosX, buttonPosY);

            _button.Update(gameTime);
        }

        public override void Render(Engine.Graphics.Sprites sprites)
        {
            if (!_isVisible)
            {
                return;
            }

            base.Render(sprites);

            _button.Render(sprites);
        }
    }
}