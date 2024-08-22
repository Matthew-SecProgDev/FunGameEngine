using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.Buttons.Models
{
    public sealed class TexturedButtonOption : BaseButtonOption
    {
        [MaybeNull]
        public Texture2D HoverTexture { get; set; }

        [MaybeNull]
        public Texture2D PressedTexture { get; set; }
    }
}