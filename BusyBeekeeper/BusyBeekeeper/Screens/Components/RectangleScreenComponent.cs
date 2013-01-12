using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BusyBeekeeper.Screens.Components
{
    internal abstract class RectangleScreenComponent : ScreenComponent
    {
        public Vector2 Size
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public bool HitTest(float x, float y)
        {
            var lPosition = this.Position;
            var lSize = this.Size;

            return ((lPosition.X <= x) && (x <= lPosition.X + lSize.X))
                && ((lPosition.Y <= y) && (y <= lPosition.Y + lSize.Y));
        }
    }
}
