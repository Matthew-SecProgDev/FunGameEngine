using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine
{
    public static class Extensions
    {
        public static void ToggleFullScreen(this GraphicsDeviceManager graphics)
        {
            graphics.HardwareModeSwitch = false;
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();
        }

        public static void SetRelativeBackBufferSize(this GraphicsDeviceManager graphics, float ratio)
        {
            ratio = FunMath.Clamp(ratio, 0.25f, 1f);

            var displayMode = graphics.GraphicsDevice.DisplayMode;

            graphics.PreferredBackBufferWidth = (int)MathF.Round(displayMode.Width * ratio);
            graphics.PreferredBackBufferHeight = (int)MathF.Round(displayMode.Height * ratio);
            graphics.ApplyChanges();
        }

        public static string GetGraphicsDeviceName(this GraphicsDeviceManager graphics)
        {
            return graphics.GraphicsDevice.Adapter.Description;
        }

        public static void GetCurrentDisplaySize(this GraphicsDeviceManager graphics, out int width, out int height)
        {
            var displayMode = graphics.GraphicsDevice.Adapter.CurrentDisplayMode;
            width = displayMode.Width;
            height = displayMode.Height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetElapsedSeconds(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetTotalSeconds(this GameTime gameTime)
        {
            return (float)gameTime.TotalGameTime.TotalSeconds;
        }
    }
}