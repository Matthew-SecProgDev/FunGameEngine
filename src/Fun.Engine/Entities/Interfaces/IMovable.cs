using Microsoft.Xna.Framework;

namespace Fun.Engine.Entities.Interfaces
{
    public interface IMovable
    {
        Vector2 MovementDirection { get; set; }
    }
}