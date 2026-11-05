using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.Entities.Interfaces
{
    public interface IRenderable
    {
        Texture2D Texture { get; }

        void Render(Graphics.Sprites sprites);
    }
}