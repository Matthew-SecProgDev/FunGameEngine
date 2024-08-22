using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.Entities.Interfaces
{
    public interface IRenderable
    {
        Texture2D Texture { get; }

        /// <summary>
        /// Renders an object within a game state and displays it on the screen.
        /// </summary>
        /// <param name="sprites"></param>
        void Render(Graphics.Sprites sprites);
    }
}