using Microsoft.Xna.Framework;

namespace TccLib.Xna.Framework
{
    /// <summary>
    /// A collection of helper methods based on the Vector2 class.
    /// </summary>
    public static class VectorHelper
    {
        /// <summary>
        /// Checks if a given point is within the boundary of a rectangle defined by the given position and size.
        /// </summary>
        /// <param name="position">The position of the rectangle.</param>
        /// <param name="size">The size of the rectangle.</param>
        /// <param name="point">The point to check.</param>
        /// <returns>True if the point lies within the rectangle, false otherwise.</returns>
        public static bool RectangleContains(Vector2 position, Vector2 size, Vector2 point)
        {
            return RectangleContains(position, size, point.X, point.Y);
        }

        /// <summary>
        /// Checks if a given point is within the boundary of a rectangle defined by the given position and size.
        /// </summary>
        /// <param name="position">The position of the rectangle.</param>
        /// <param name="size">The size of the rectangle.</param>
        /// <param name="x">The x-coordinate of the point to check.</param>
        /// <param name="y">The y-coordinate of the point to check.</param>
        /// <returns>True if the point lies within the rectangle, false otherwise.</returns>
        public static bool RectangleContains(Vector2 position, Vector2 size, float x, float y)
        {
            return
                (position.X <= x) && (x <= position.X + size.X) &&
                (position.Y <= y) && (y <= position.Y + size.Y);

        }
    }
}
