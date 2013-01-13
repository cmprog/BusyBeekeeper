using System;
using Microsoft.Xna.Framework;

namespace BusyBeekeeper
{
    internal static class VectorUtilities
    {
        public static bool HitTest(Vector2 position, Vector2 size, Vector2 point)
        {
            return HitTest(point, size, point.X, point.Y);
        }

        public static bool HitTest(Vector2 position, Vector2 size, float x, float y)
        {
            return ((position.X <= x) && (x <= position.X + size.X))
                && ((position.Y <= y) && (y <= position.Y + size.Y));
        }
    }
}
