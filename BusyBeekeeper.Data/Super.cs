using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// A super is a component of a single bee hive. They come in different sizes and can
    /// be painted.
    /// </summary>
    public class Super : Component
    {
        /// <summary>
        /// Initializes a new instance of the Super class.
        /// </summary>
        public Super()
        {
        }

        /// <summary>
        /// Gets the resource ID of this super - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
