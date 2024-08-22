using Microsoft.Xna.Framework;

namespace Fun.Engine.Geometry
{
    public readonly struct Transform2D
    {
        public readonly float PosX;
        public readonly float PosY;
        public readonly float SinScaleX;
        public readonly float CosScaleX;
        public readonly float SinScaleY;
        public readonly float CosScaleY;

        public Transform2D(Vector2 position, float angle, Vector2 scale)
        {
            var sin = MathF.Sin(angle);
            var cos = MathF.Cos(angle);

            PosX = position.X;
            PosY = position.Y;
            SinScaleX = sin * scale.X;
            CosScaleX = cos * scale.X;
            SinScaleY = sin * scale.Y;
            CosScaleY = cos * scale.Y;
        }

        public Transform2D(Vector2 position, float angle, float scale)
        {
            var sin = MathF.Sin(angle);
            var cos = MathF.Cos(angle);

            PosX = position.X;
            PosY = position.Y;
            SinScaleX = sin * scale;
            CosScaleX = cos * scale;
            SinScaleY = sin * scale;
            CosScaleY = cos * scale;
        }

        public Matrix ToMatrix()
        {
            var result = Matrix.Identity;
            result.M11 = CosScaleX;
            result.M12 = SinScaleY;
            result.M21 = -SinScaleX;
            //result.M12 = -SinScaleY;//check it
            //result.M21 = SinScaleX;//check it
            result.M22 = CosScaleY;
            result.M41 = PosX;
            result.M42 = PosY;

            return result;
        }
    }
}