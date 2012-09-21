using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Lawn mowers are used to help keep the bee yards from becoming overrun with grass and such.
    /// </summary>
    public class LawnMower : Component
    {
        /// <summary>
        /// Initializes a new instance of the LawnMower class.
        /// </summary>
        public LawnMower()
        {
        }

        /// <summary>
        /// Gets the resource ID of this lawn mower - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
