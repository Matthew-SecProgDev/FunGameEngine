using Fun.Engine.Entities.Interfaces;
using Microsoft.Xna.Framework;

namespace Fun.Engine.Entities
{
    public abstract class BaseGameObject : BaseObject, IMovable, IDestroyable
    {
        public Vector2 ScreenPosition { get; set; } = Vector2.Zero;

        public Vector2 MovementDirection { get; set; } = Vector2.Zero;

        public bool Destroyed { get; private set; }

        public override void Render(Graphics.Sprites sprites)
        {
            if (!Destroyed)
            {
                sprites.Draw(Texture, Position + ScreenPosition, null, Color.White);
            }
        }

        public virtual void Destroy()
        {
            Destroyed = true;
        }

        public event ChildObjectHandler? AddObjectHandler;
        protected void AddObject(BaseObject @object)
        {
            AddObjectHandler?.Invoke(@object);
        }

        public event ChildObjectHandler? RemoveObjectHandler;
        protected void RemoveObject(BaseObject @object)
        {
            RemoveObjectHandler?.Invoke(@object);
        }
    }
}