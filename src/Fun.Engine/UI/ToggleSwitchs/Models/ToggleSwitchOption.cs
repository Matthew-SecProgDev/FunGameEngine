using Microsoft.Xna.Framework.Graphics;

namespace Fun.Engine.UI.ToggleSwitchs.Models
{
    public enum DefaultState
    {
        OnSwitch = 0,

        OffSwitch = 1
    }

    public sealed class ToggleSwitchOption
    {
        public required Texture2D OnTexture { get; set; }

        public required Texture2D OffTexture { get; set; }

        public DefaultState State { get; set; } = DefaultState.OffSwitch;
    }
}