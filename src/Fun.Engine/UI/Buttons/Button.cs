using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.Buttons
{
    public sealed class Button : BaseUIObject
    {
        private readonly Texture2D? _hoverTexture;
        private readonly Texture2D? _pressedTexture;
        private readonly string? _text;
        private readonly SpriteFont? _font;
        private readonly Color _penColor;
        private readonly Action _onClick;
        private Rectangle _bounds;
        private bool _isHovering;
        private bool _isPressed;

        public Button(Vector2 size, Models.TexturedButtonOption texturedButtonOption, Vector2 position) 
            : base(size, position)
        {
            Texture = texturedButtonOption.NormalTexture;
            _hoverTexture = texturedButtonOption.HoverTexture;
            _pressedTexture = texturedButtonOption.PressedTexture;
            _onClick = texturedButtonOption.OnClick;
        }

        public Button(Vector2 size, Models.TextButtonOption textButtonOption, Vector2 position) : base(size, position)
        {
            Texture = textButtonOption.NormalTexture;
            _text = textButtonOption.Text;
            _penColor = textButtonOption.PenColor;
            _font = textButtonOption.Font;
            _onClick = textButtonOption.OnClick;
        }

        public override void Initialize()
        {
            _bounds = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }

        public override void Update(GameTime gameTime)
        {
            var mouse = Input.Mouse.Instance;
            var mousePressed = mouse.IsLeftButtonPressed();
            var mouseReleased = mouse.IsLeftButtonReleased();

            // Check if the mouse is over the button
            //_isHovering = Bounds.Contains(inputManager.GetMousePosition());
            _isHovering = _bounds.Contains(mouse.WorldPosition);
            if (_isHovering)
            {
                if (mousePressed)
                {
                    _isPressed = true;
                }

                if (mouseReleased && _isPressed)
                {
                    _onClick?.Invoke();
                    _isPressed = false;
                }
            }
            else
            {
                _isPressed = false;
            }
        }

        public override void Render(Graphics.Sprites sprites)
        {
            if (_font != null)
            {
                DrawButtonWithText(sprites);

                return;
            }

            var texture = Texture;
            if (_isPressed)
            {
                if (_pressedTexture != null)
                {
                    texture = _pressedTexture;
                }
            }
            else if (_isHovering)
            {
                if (_hoverTexture != null)
                {
                    texture = _hoverTexture;
                }
            }

            sprites.Draw(texture, _bounds, null, Color.White);
        }

        public void UpdateButtonPosition(int x, int y)
        {
            //TODO: Test it
            //TODO?? since we have PositionX & PositionY for changing values,
            //TODO?? so I must check later, do I need this function in the future or not 
            _bounds.X = x;
            _bounds.Y = y;
        }

        private void DrawButtonWithText(Graphics.Sprites sprites)
        {
            var color = Color.White;

            if (_isHovering)
            {
                color = Color.Gray;
            }

            sprites.Draw(Texture, _bounds, null, color);

            if (string.IsNullOrWhiteSpace(_text))
            {
                return;
            }

            var (x, y) = _font!.MeasureString(_text);
            x = _bounds.X + _bounds.Width / 2f - x / 2;
            y = _bounds.Y + _bounds.Height / 2f - y / 2;

            sprites.DrawString(_font, _text, new Vector2(x, y), _penColor);
        }
    }
}