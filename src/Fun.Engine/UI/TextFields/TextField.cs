using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fun.Engine.UI.TextFields
{
    public sealed class TextField : BaseUIObject
    {
        // we can add extra configuration to set limitation on count of incoming or
        // make limit on kind of value, for example Textfield takes only integer values or
        // take custom validation from side of developer
        private const string CursorCharacter = "|";
        private readonly SpriteFont _font;
        private readonly Color _penColor;
        private readonly StringBuilder _textBuilder;
        private bool _hasFocus;
        private double _cursorTimer;
        private bool _cursorVisible;
        private int _cursorPosition;

        public TextField(Vector2 size, Models.TextFieldOption textFieldOption, Vector2 position) : base(size, position)
        {
            Texture = textFieldOption.Texture;
            _font = textFieldOption.Font;
            _penColor = textFieldOption.PenColor;
            _textBuilder = new StringBuilder();
        }

        private Rectangle Bounds => new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

        public string Text => _textBuilder.ToString();

        public override void Update(GameTime gameTime)
        {
            var keyboard = Input.Keyboard.Instance;
            var mouse = Input.Mouse.Instance;

            var mousePressed = mouse.IsLeftButtonPressed();

            //var isHovering = Bounds.Contains(inputManager.GetMousePosition());
            var isHovering = Bounds.Contains(mouse.WorldPosition);
            if (mousePressed && isHovering)
            {
                _hasFocus = true;
            }
            else if (mousePressed && !isHovering)
            {
                _hasFocus = false;
            }

            if (_hasFocus)
            {
                // Handle text input
                var pressedKeys = keyboard.GetPressedKeys();
                foreach (var key in pressedKeys)
                {
                    if (keyboard.IsKeyPressed(key))
                    {
                        var length = _textBuilder.Length;

                        if (key == Keys.Left && _cursorPosition > 0)
                        {
                            _cursorPosition--;
                        }
                        else if (key == Keys.Right && _cursorPosition < length)
                        {
                            _cursorPosition++;
                        }
                        else if (key == Keys.Back && _cursorPosition > 0 && _cursorPosition <= length)
                        {
                            // if text is selected so condition is different
                            _textBuilder.Remove(_cursorPosition - 1, 1);
                            _cursorPosition--;
                        }
                        else if (key == Keys.Delete && _cursorPosition < length)
                        {
                            _textBuilder.Remove(_cursorPosition, 1);
                        }
                        else if (key == Keys.Space)
                        {
                            _textBuilder.Insert(_cursorPosition, ' ');
                            _cursorPosition++;
                        }
                        else if (_font.Characters.Contains((char)key))
                        {
                            var isNumeric = IsNumeric(key);

                            if (_cursorPosition == 0 && isNumeric)
                            {
                                throw new Exception("");
                            }
                            else
                            {
                                if (isNumeric)
                                {
                                    _textBuilder.Insert(_cursorPosition, (char)key);
                                    _cursorPosition++;
                                }
                                else if (IsAlphabetic(key))
                                {
                                    var @char = (char)key;

                                    if ((keyboard.IsCapsLockActive &&
                                         (keyboard.IsKeyDown(Keys.LeftShift) ||
                                          keyboard.IsKeyDown(Keys.RightShift))) ||
                                        (!keyboard.IsCapsLockActive && !keyboard.IsKeyDown(Keys.LeftShift) &&
                                         !keyboard.IsKeyDown(Keys.RightShift)))
                                    {
                                        _textBuilder.Insert(_cursorPosition, char.ToLower(@char));
                                    }
                                    else
                                    {
                                        _textBuilder.Insert(_cursorPosition, @char);
                                    }

                                    _cursorPosition++;
                                }
                                else
                                {
                                    throw new Exception("");
                                }
                            }
                        }
                    }
                }

                // Cursor blink logic
                _cursorTimer += gameTime.GetElapsedSeconds();
                if (_cursorTimer >= 0.5)
                {
                    _cursorVisible = !_cursorVisible;
                    _cursorTimer = 0;
                }
            }
        }

        public override void Render(Graphics.Sprites sprites)
        {
            sprites.Draw(Texture, Bounds, null, Color.White);

            var padding = Bounds.Width * 0.05f;
            var maxWidth = Bounds.Width - 2 * padding - _font.MeasureString(CursorCharacter).X;

            //TODO: should I have to create StringBuilder each time?
            var stringBuilder = new StringBuilder(_textBuilder.ToString());
            if (_font.MeasureString(stringBuilder).X > maxWidth)
            {
                var viewTextWidthStart = 0;
                var viewTextWidthEnd = stringBuilder.Length - 1;
                while (_font.MeasureString(stringBuilder).X > maxWidth && viewTextWidthStart < _cursorPosition)
                {
                    viewTextWidthStart++;
                    stringBuilder.Remove(0, 1);
                }

                while (_font.MeasureString(stringBuilder).X > maxWidth)
                {
                    viewTextWidthEnd--;
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }

                if (_cursorVisible && _hasFocus)
                {
                    if (_cursorPosition - 1 == viewTextWidthEnd)
                    {
                        stringBuilder.Append(CursorCharacter[0]);
                    }
                    else if (_cursorPosition == viewTextWidthStart)
                    {
                        stringBuilder.Insert(0, CursorCharacter);
                    }
                    else
                    {
                        stringBuilder.Insert(_cursorPosition - viewTextWidthStart, CursorCharacter);
                    }
                }
            }
            else
            {
                if (_cursorVisible && _hasFocus)
                {
                    if (_cursorPosition == stringBuilder.Length)
                    {
                        stringBuilder.Append(CursorCharacter[0]);
                    }
                    else if (_cursorPosition == 0)
                    {
                        stringBuilder.Insert(0, CursorCharacter);
                    }
                    else
                    {
                        stringBuilder.Insert(_cursorPosition, CursorCharacter);
                    }
                }
            }

            var x = Bounds.X + padding;
            var y = Bounds.Y + Bounds.Height / 2f - _font.MeasureString(stringBuilder).Y / 2;

            sprites.DrawString(_font, stringBuilder, new Vector2(x, y), _penColor);
        }

        private static bool IsNumeric(Keys key)
        {
            return key is >= Keys.D0 and <= Keys.D9;
        }

        private static bool IsAlphabetic(Keys key)
        {
            return key is >= Keys.A and <= Keys.Z;
        }
    }
}