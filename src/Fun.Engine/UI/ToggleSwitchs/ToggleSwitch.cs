using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.ToggleSwitchs
{
    public sealed class ToggleSwitch : BaseUIObject
    {
        private readonly Texture2D _onTexture;
        private readonly Texture2D _offTexture;

        public ToggleSwitch(Vector2 size, Models.ToggleSwitchOption option, Vector2 position) : base(size, position)
        {
            _onTexture = option.OnTexture;
            _offTexture = option.OffTexture;

            this.IsOn = option.State == Models.DefaultState.OnSwitch;
        }

        private Rectangle Bounds => new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

        public bool IsOn { get; private set; }

        public override void Update(GameTime gameTime)
        {
            var mouse = Input.Mouse.Instance;
            var mousePressed = mouse.IsLeftButtonPressed();
            //if (Bounds.Contains(inputManager.GetMousePosition()) && mousePressed)
            if (this.Bounds.Contains(mouse.WorldPosition) && mousePressed)
            {
                this.IsOn = !IsOn;
            }
        }

        public override void Render(Graphics.Sprites sprites)
        {
            sprites.Draw(this.IsOn ? _onTexture : _offTexture, this.Bounds, null, Color.White);
        }
    }
}