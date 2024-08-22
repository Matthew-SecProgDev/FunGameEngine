using System.Diagnostics.CodeAnalysis;
using Fun.Engine.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.Graphics
{
    public sealed class Shapes : IDisposable
    {
        //private readonly Game _game;
        private readonly Sprites _sprites;
        private readonly BasicEffect _effect;

        private readonly VertexPositionColor[] _vertices;
        private readonly int[] _indices;

        private Camera? _camera;

        private int _shapeCount;
        private int _vertexCount;
        private int _indexCount;

        private bool _isStarted;
        private bool _isDisposed;

        public const float MinLineThickness = 1f;
        public const float MaxLineThickness = 10f;

        //public Shapes([NotNull]Game? game)
        public Shapes(Sprites sprites)
        {
            //ArgumentNullException.ThrowIfNull(game);
            ArgumentNullException.ThrowIfNull(sprites);

            //_game = game;
            _sprites = sprites;
            //_effect = new BasicEffect(game.GraphicsDevice)
            _effect = new BasicEffect(sprites.GraphicsDevice)
            {
                FogEnabled = false,
                TextureEnabled = false,
                LightingEnabled = false,
                VertexColorEnabled = true,
                World = Matrix.Identity,
                Projection = Matrix.Identity,
                View = Matrix.Identity
            };

            const int maxVertexCount = 1024;
            const int maxIndexCount = maxVertexCount * 3;

            _vertices = new VertexPositionColor[maxVertexCount];
            _indices = new int[maxIndexCount];

            _shapeCount = 0;
            _vertexCount = 0;
            _indexCount = 0;

            _camera = null;

            _isStarted = false;
            _isDisposed = false;
        }

        public void Begin(Camera? camera)
        {
#if DEBUG
            if (_isStarted)
            {
                const string mas = "Batching is already started.";
                throw new Exception(mas);
            }
#endif

            // Set the blend state for transparency
            //_game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            if (camera is null)
            {
                //var viewport = _game.GraphicsDevice.Viewport;
                var viewport = _sprites.GraphicsDevice.Viewport;
                _effect.Projection = Matrix.CreateOrthographicOffCenter(0f, viewport.Width, 0f, viewport.Height, 0f, 1f);
                _effect.View = Matrix.Identity;
            }
            else
            {
                camera.UpdateMatrices();

                _effect.Projection = camera.Projection;
                _effect.View = camera.View;
            }

            //var viewport = _game.GraphicsDevice.Viewport;
            //_effect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, 0, viewport.Height, 0f, 1f);

            _camera = camera;

            _isStarted = true;
        }

        public void End()
        {
            this.Flush();

            _isStarted = false;
        }

        public void DrawFilledRectangle(float x, float y, float width, float height, Color color)
        {
            this.EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            this.EnsureSpace(shapeVertexCount, shapeIndexCount);

            var left = x;
            var right = x + width;
            var bottom = y;
            var top = y + height;

            var a = new Vector2(left, top);
            var b = new Vector2(right, top);
            var c = new Vector2(right, bottom);
            var d = new Vector2(left, bottom);

            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;

            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(a, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(b, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(c, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(d, 0f), color);

            _shapeCount++;
        }

        public void DrawLine(Vector2 a, Vector2 b, float thickness, Color color)
        {
            this.EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            this.EnsureSpace(shapeVertexCount, shapeIndexCount);

            thickness = FunMath.Clamp(thickness, MinLineThickness, MaxLineThickness);

            //thickness = MathHelper.Clamp(thickness, 2, 11);
            //thickness++;

            if (_camera != null)
            {
                thickness *= _camera.Z / _camera.BaseZ;
            }

            var halfThickness = thickness / 2f;

            var e1 = b - a;
            FunMath.Normalize(ref e1.X, ref e1.Y);
            e1 *= halfThickness;

            var e2 = -e1;
            var n1 = new Vector2(-e1.Y, e1.X);  //it must be checked because of -e1.Y
            var n2 = -n1;

            var q1 = a + n1 + e2;
            var q2 = b + n1 + e1;
            var q3 = b + n2 + e1;
            var q4 = a + n2 + e2;

            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;

            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q1, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q2, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q3, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q4, 0f), color);

            _shapeCount++;
        }

        public void DrawLine(float aX, float aY, float bX, float bY, float thickness, Color color)
        {
            this.EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            this.EnsureSpace(shapeVertexCount, shapeIndexCount);

            thickness = FunMath.Clamp(thickness, MinLineThickness, MaxLineThickness);

            //thickness = MathHelper.Clamp(thickness, 2, 11);
            //thickness++;

            if (_camera != null)
            {
                thickness *= _camera.Z / _camera.BaseZ;
            }

            var halfThickness = thickness / 2f;

            // Creating edges
            var e1X = bX - aX;
            var e1Y = bY - aY;

            FunMath.Normalize(ref e1X, ref e1Y);

            e1X *= halfThickness;
            e1Y *= halfThickness;

            var e2X = -e1X;
            var e2Y = -e1Y;

            var n1X = -e1Y; //it must be checked because of -e1Y
            var n1Y = e1X;

            var n2X = -n1X;
            var n2Y = -n1Y;

            // Creating x and y of quadrilaterals
            var q1X = aX + n1X + e2X;
            var q1Y = aY + n1Y + e2Y;

            var q2X = bX + n1X + e1X;
            var q2Y = bY + n1Y + e1Y;

            var q3X = bX + n2X + e1X;
            var q3Y = bY + n2Y + e1Y;

            var q4X = aX + n2X + e2X;
            var q4Y = aY + n2Y + e2Y;

            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;

            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q1X, q1Y,  0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q2X, q2Y,  0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q3X, q3Y,  0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q4X, q4Y,  0f), color);

            _shapeCount++;
        }

        public void DrawRectangle(float x, float y, float width, float height, float thickness, Color color)
        {
            var left = x;
            var right = x + width;
            var bottom = y;
            var top = y + height;

            this.DrawLine(left, top, right, top, thickness, color);
            this.DrawLine(right, top, right, bottom, thickness, color);
            this.DrawLine(right, bottom, left, bottom, thickness, color);
            this.DrawLine(left, bottom, left, top, thickness, color);
        }

        public void DrawCircle(float x, float y, float radius, int points, float thickness, Color color)
        {
            const int minPoints = 3;
            const int maxPoints = 256;

            points = FunMath.Clamp(points, minPoints, maxPoints);

            var deltaAngle = Microsoft.Xna.Framework.MathHelper.TwoPi / points;
            var angle = 0f;

            for (var i = 0; i < points; i++)
            {
                var aX = MathF.Sin(angle) * radius + x;
                var aY = MathF.Cos(angle) * radius + y;

                angle += deltaAngle;

                var bX = MathF.Sin(angle) * radius + x;
                var bY = MathF.Cos(angle) * radius + y;

                this.DrawLine(aX, aY, bX, bY, thickness, color);
            }
        }

        public void DrawCircleFast(float x, float y, float radius, int points, float thickness, Color color)
        {
            const int minPoints = 3;
            const int maxPoints = 256;

            points = FunMath.Clamp(points, minPoints, maxPoints);

            var rotation = Microsoft.Xna.Framework.MathHelper.TwoPi / points;

            var sin = MathF.Sin(rotation);
            var cos = MathF.Cos(rotation);

            var aX = radius;
            var aY = 0f;

            float bX, bY;
            for (var i = 0; i < points; i++)
            {
                bX = cos * aX - sin * aY;
                bY = sin * aX + cos * aY;

                this.DrawLine(aX + x, aY + y, bX + x, bY + y, thickness, color);

                aX = bX;
                aY = bY;
            }
        }

        public void DrawFilledCircle(float x, float y, float radius, int points, Color color)
        {
            this.EnsureStarted();

            const int minPoints = 3;
            const int maxPoints = 256;

            var shapeVertexCount = FunMath.Clamp(points, minPoints, maxPoints);
            var shapeTriangleCount = shapeVertexCount - 2;
            var shapeIndexCount = shapeTriangleCount * 3;

            this.EnsureSpace(shapeVertexCount, shapeIndexCount);

            var index = 1;
            for (var i = 0; i < shapeTriangleCount; i++)
            {
                _indices[_indexCount++] = 0 + _vertexCount;
                _indices[_indexCount++] = index + _vertexCount;
                _indices[_indexCount++] = index + 1 + _vertexCount;

                index++;
            }

            //for (var i = 0; i < shapeTriangleCount; i++)
            //{
            //    _indices[_indexCount++] = 0 + _vertexCount;
            //    _indices[_indexCount++] = i + 1 + _vertexCount;
            //    _indices[_indexCount++] = i + 2 + _vertexCount;
            //}

            var rotation = Microsoft.Xna.Framework.MathHelper.TwoPi / points;

            var sin = MathF.Sin(rotation);
            var cos = MathF.Cos(rotation);

            var aX = radius;
            var aY = 0f;

            for (var i = 0; i < shapeVertexCount; i++)
            {
                var x1 = aX;
                var y1 = aY;

                _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(x1 + x, y1 + y, 0f), color);

                aX = cos * x1 - sin * y1;
                aY = sin * x1 + cos * y1;
            }

            _shapeCount++;
        }

        public void DrawPolygon(Vector2[] vertices, Matrix transform, float thickness, Color color)
        {
#if DEBUG
            ArgumentNullException.ThrowIfNull(vertices);

            if (vertices.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(vertices));
            }
#endif

            var verticesLength = vertices.Length;
            for (var i = 0; i < verticesLength; i++)
            {
                var a = vertices[i];
                var b = vertices[(i + 1) % verticesLength];

                a = Vector2.Transform(a, transform);
                b = Vector2.Transform(b, transform);

                this.DrawLine(a, b, thickness, color);
            }
        }

        public void DrawPolygon(Vector2[] vertices, Transform2D transform, float thickness, Color color)
        {
#if DEBUG
            ArgumentNullException.ThrowIfNull(vertices);

            if (vertices.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(vertices));
            }
#endif

            var verticesLength = vertices.Length;
            for (var i = 0; i < verticesLength; i++)
            {
                var a = vertices[i];
                var b = vertices[(i + 1) % verticesLength];

                a = FunMath.Transform(a, transform);
                b = FunMath.Transform(b, transform);

                this.DrawLine(a, b, thickness, color);
            }
        }

        public void DrawFilledPolygon(
            Vector2[] vertices, 
            int[] triangleIndices, 
            Transform2D transform, 
            Color color, 
            float transparency = 0.1f)
        {
#if DEBUG
            ArgumentNullException.ThrowIfNull(vertices);
            ArgumentNullException.ThrowIfNull(triangleIndices);

            if (vertices.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(vertices));
            }

            if (triangleIndices.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(triangleIndices));
            }
#endif
            //maybe we should make transparency by another way, for increase performance and simplification
            transparency = FunMath.Clamp(transparency, 0.1f, 1f);

            this.EnsureStarted();
            this.EnsureSpace(vertices.Length, triangleIndices.Length);

            for (var i = 0; i < triangleIndices.Length; i++)
            {
                _indices[_indexCount++] = triangleIndices[i] + _vertexCount;
            }

            for (var i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                vertex = FunMath.Transform(vertex, transform);
                _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(vertex.X, vertex.Y, 0f), color * transparency);
            }

            _shapeCount++;
        }

        public void DrawPolygonTriangulation(
            Vector2[] vertices,
            int[] triangleIndices,
            Transform2D transform,
            Color color)
        {
#if DEBUG
            ArgumentNullException.ThrowIfNull(vertices);
            ArgumentNullException.ThrowIfNull(triangleIndices);

            if (vertices.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(vertices));
            }

            if (triangleIndices.Length < 3)
            {
                throw new ArgumentOutOfRangeException(nameof(triangleIndices));
            }
#endif

            for (var i = 0; i < triangleIndices.Length; i += 3)
            {
                var a = triangleIndices[i];
                var b = triangleIndices[i + 1];
                var c = triangleIndices[i + 2];

                var aV = vertices[a];
                var bV = vertices[b];
                var cV = vertices[c];

                aV = FunMath.Transform(aV, transform);
                bV = FunMath.Transform(bV, transform);
                cV = FunMath.Transform(cV, transform);

                this.DrawLine(aV, bV, 1f, color);
                this.DrawLine(bV, cV, 1f, color);
                this.DrawLine(cV, aV, 1f, color);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    _effect.Dispose();
                }

                // TODO: Override finalizer (destructor) only if there is code to free unmanaged resources
                // Free unmanaged resources (unmanaged objects)
                // Set large fields to null

                _isDisposed = true;
            }
        }

        private void Flush()
        {
            if (_shapeCount == 0)
            {
                return;
            }

            this.EnsureStarted();

            foreach (var effectPass in _effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                //_game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                _sprites.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                    _vertices,
                    0,
                    _vertexCount,
                    _indices,
                    0,
                    _indexCount / 3);
                //_game.GraphicsDevice.DrawUserIndexedPrimitives()
            }

            _shapeCount = 0;
            _vertexCount = 0;
            _indexCount = 0;
        }

        private void EnsureStarted()
        {
            if (_isStarted)
            {
                return;
            }

            const string msg = "Batching was never started.";
            throw new Exception(msg);
        }

        private void EnsureSpace(int shapeVertexCount, int shapeIndexCount)
        {
            if (shapeVertexCount > _vertices.Length)
            {
                throw new ArgumentOutOfRangeException(null, "Maximum shape vertex count is: " + _vertices.Length);
            }

            if (shapeIndexCount > _indices.Length)
            {
                throw new ArgumentOutOfRangeException(null, "Maximum shape index count is: " + _indices.Length);
            }

            if (_vertexCount + shapeVertexCount > _vertices.Length ||
                _indexCount + shapeIndexCount > _indices.Length)
            {
                this.Flush();
            }
        }
    }
}