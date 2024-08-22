using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using FunMath = Fun.Engine.Utilities.MathHelper;

namespace Fun.Engine.Physics
{
    public enum WindingOrder
    {
        Invalid = 0,

        CounterClockWise = 1,

        Clockwise = 2
    }

    public static class PolygonHelper
    {
        public static float FindPolygonArea([NotNull]Vector2[]? vertices)
        {
#if DEBUG
            ArgumentNullException.ThrowIfNull(vertices);

            if (vertices.Length < 3)
            {
                throw new ArgumentException("A polygon must have at least three vertices.");
            }
#endif

            var totalArea = 0f;

            for (var i = 0; i < vertices.Length; i++)
            {
                var a = vertices[i];
                var b = vertices[(i + 1) % vertices.Length];

                var dX = b.X - a.X;
                var dY = (a.Y + b.Y) / 2f;

                var area = dY * dX;
                totalArea += area;
            }

            return MathF.Abs(totalArea);
        }

        public static bool Triangulate(Vector2[]? vertices, out int[]? triangleIndices, out string errorMessage)
        {
            triangleIndices = null;
            errorMessage = string.Empty;

            if (vertices is null)
            {
                errorMessage = "The vertex list is null.";
                return false;
            }

            if (vertices.Length < 3)
            {
                errorMessage = "The vertex list must have at least three vertices.";
                return false;
            }

            if (vertices.Length > 1024)
            {
                errorMessage = "The max vertex list length is 1024.";
                return false;
            }

            if (!PolygonHelper.IsSimplePolygon(vertices))
            {
                errorMessage = "The vertex list does not define a simple polygon.";
                return false;
            }

            if (PolygonHelper.ContainsCollinearEdges(vertices))
            {
                errorMessage = "The vertex list contains collinear edges.";
                return false;
            }

            PolygonHelper.ComputePolygonArea(vertices, out var area, out var windingOrder);

            if (windingOrder is WindingOrder.Invalid)
            {
                errorMessage = "The vertex list does not contain a valid polygon.";
                return false;
            }

            if (windingOrder is WindingOrder.CounterClockWise)
            {
                Array.Reverse(vertices);
            }

            var polygonIndices = new List<int>();
            for (var i = 0; i < vertices.Length; i++)
            {
                polygonIndices.Add(i);
            }

            var triangleCount = vertices.Length - 2;

            triangleIndices = new int[triangleCount * 3];
            var triangleIndex = 0;

            // Finding all ears in polygonIndices
            while (polygonIndices.Count > 3)
            {
                for (var i = 0; i < polygonIndices.Count; i++)
                {
                    var currentIndex = polygonIndices[i];
                    var previousIndex = FunMath.GetItem(polygonIndices, i - 1);
                    var nextIndex = FunMath.GetItem(polygonIndices, i + 1);

                    var currentVertex = vertices[currentIndex];
                    var previousVertex = vertices[previousIndex];
                    var nextVertex = vertices[nextIndex];

                    var currentVertexToPreviousVertex = previousVertex - currentVertex;
                    var currentVertexToNextVertex = nextVertex - currentVertex;

                    // Test1: Checking whether currentVertex is a convex vertex or not?!
                    if (FunMath.CrossProduct(currentVertexToPreviousVertex, currentVertexToNextVertex) < 0f)
                    {
                        // currentVertex is a reflex vertex, that means internal angle is greater than 180 degree
                        continue;
                    }

                    // Test2: Checking whether an ear contains any polygon vertices or not?!
                    var isEar = true;
                    for (var j = 0; j < vertices.Length; j++)
                    {
                        if (j == previousIndex || j == currentIndex || j == nextIndex)
                        {
                            continue;
                        }

                        var point = vertices[j];
                        // Passing these three vertices as a triangle (defined by previousVertex,currentVertex,nextVertex)
                        //      in clockwise winding order to the function
                        if (PolygonHelper.IsPointInTriangle(previousVertex, currentVertex, nextVertex, point))
                        {
                            // currentVertex is not an ear
                            isEar = false;
                            break;
                        }
                    }

                    if (isEar)
                    {
                        // Adding these three indices in clockwise winding order into triangleIndices
                        triangleIndices[triangleIndex++] = previousIndex;
                        triangleIndices[triangleIndex++] = currentIndex;
                        triangleIndices[triangleIndex++] = nextIndex;

                        // Removing the ear from polygonIndices
                        polygonIndices.RemoveAt(i);

                        break;
                    }
                }
            }

            triangleIndices[triangleIndex++] = polygonIndices[0];
            triangleIndices[triangleIndex++] = polygonIndices[1];
            triangleIndices[triangleIndex] = polygonIndices[2];


            return true;
        }

        private static bool IsSimplePolygon(Vector2[] vertices)
        {
            throw new NotImplementedException();
        }

        private static bool ContainsCollinearEdges(Vector2[] vertices)
        {
            throw new NotImplementedException();
        }

        private static void ComputePolygonArea(Vector2[] vertices, out float area, out WindingOrder windingOrder)
        {
            throw new NotImplementedException();
        }

        private static bool IsPointInTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 point)
        {
            var ab = b - a;
            var bc = c - b;
            var ca = a - c;

            var ap = point - a;
            var bp = point - b;
            var cp = point - c;

            var crossProduct1 = FunMath.CrossProduct(ab, ap);
            var crossProduct2 = FunMath.CrossProduct(bc, bp);
            var crossProduct3 = FunMath.CrossProduct(ca, cp);

            if (crossProduct1 > 0f || crossProduct2 > 0f || crossProduct3 > 0f)
            {
                return false;
            }

            return true;
        }
    }
}