using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Defines the properties for a bee suit. They are handy to have when working with the bees.
    /// </summary>
    public class BeeSuit : Component
    {
        /// <summary>
        /// Initializes a new instance of the BeeSuit class.
        /// </summary>
        public BeeSuit()
        {
        }

        /// <summary>
        /// Gets the resource ID of this suit - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
