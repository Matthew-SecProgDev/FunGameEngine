using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.UI.Sliders
{
    public class Slider : BaseUIObject
    {
        private readonly Texture2D _handleTexture;
        private readonly Texture2D _fillTexture;
        private readonly float _minValue;
        private readonly float _maxValue;
        private Rectangle _handleBounds;
        private Rectangle _fillBounds;
        private bool _isDragging;
        private float _currentValue;

        private Rectangle SliderBounds => new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

        public Rectangle HandleBounds
        {
            get => _handleBounds;
            protected set => _handleBounds = value;    //maybe this property should be private, maybe not and init
        }

        public float Value
        {
            // we later can add extra feature for setting value from out via textfield inside Slider
            get => _currentValue;
            private set => _currentValue = FunMath.Clamp(value, _minValue, _maxValue);
        }

        public Slider(Vector2 size, Models.SliderOption option, Vector2 position) : base(size, position)
        {
            // Maybe user should set a value for example 20 from 100 and them we should set position of HandleBounds and the value

            Texture = option.BackgroundTexture;
            _handleTexture = option.HandleTexture;
            _fillTexture = option.FillTexture;
            _currentValue = option.InitialValue;
            _minValue = option.MinValue;
            _maxValue = option.MaxValue;

            var handleSize = option.HandleSize;
            var handlePosY = SliderBounds.Y + SliderBounds.Height / 2f - handleSize.Y / 2f;
            HandleBounds = new Rectangle((int)Position.X, (int)handlePosY, (int)handleSize.X, (int)handleSize.Y);

            var fillPosY = SliderBounds.Y + SliderBounds.Height / 2f - option.FillHeight / 2f;
            _fillBounds = new Rectangle((int)Position.X, (int)fillPosY, 0, (int)option.FillHeight);

            UpdateSliderVisuals();
        }

        public override void Update(GameTime gameTime)
        {
            var mouse = Input.Mouse.Instance;
            //var mousePosition = inputManager.GetMousePosition();
            var mousePosition = mouse.WorldPosition;
            //if (inputManager.IsLeftMouseDown() && SliderBounds.Contains(mousePosition))
            if (mouse.IsLeftButtonDown() && SliderBounds.Contains(mousePosition))
            {
                _isDragging = true;
            }
            //else if(inputManager.IsLeftMouseUp())
            else if(_isDragging && mouse.IsLeftButtonReleased())
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                var positionInBackground = mousePosition.X - SliderBounds.X;
                var percent = FunMath.Clamp(positionInBackground / SliderBounds.Width, 0f, 1f);
                Value = _minValue + percent * (_maxValue - _minValue);

                UpdateSliderVisuals();
            }
        }

        public override void Render(Graphics.Sprites sprites)
        {
            sprites.Draw(Texture, SliderBounds, null, Color.White);
            sprites.Draw(_fillTexture, _fillBounds, null, Color.White);
            sprites.Draw(_handleTexture, HandleBounds, null, Color.White);
        }

        private void UpdateSliderVisuals()
        {
            // Update handle position based on the current value
            var percent = (_currentValue - _minValue) / (_maxValue - _minValue);
            _handleBounds.X = (int)(Position.X + percent * (SliderBounds.Width - HandleBounds.Width));

            // Update fill width based on the current value
            _fillBounds.Width = (int)(_currentValue / _maxValue * SliderBounds.Width);
        }
    }
}