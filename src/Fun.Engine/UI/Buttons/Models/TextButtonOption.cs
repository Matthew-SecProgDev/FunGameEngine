using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.Buttons.Models
{
    public sealed class TextButtonOption : BaseButtonOption
    {
        [MaybeNull]
        public string Text { get; init; }

        public Color PenColor { get; init; }

        [MaybeNull]
        public SpriteFont Font { get; init; }
    }
}