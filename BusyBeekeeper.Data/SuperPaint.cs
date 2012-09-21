using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Supers can be painted. Paint is purely cosmetic.
    /// </summary>
    public class SuperPaint : Component
    {
        /// <summary>
        /// Initializes a new instance of the SuperPaint class.
        /// </summary>
        public SuperPaint()
        {
        }

        /// <summary>
        /// Gets the resource ID of this paint - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
