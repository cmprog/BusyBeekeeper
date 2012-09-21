using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Defines a screen location within a BeeYard at which a BeeHive can be located.
    /// </summary>
    public class BeeYardHiveLocation : Component
    {
        /// <summary>
        /// Initializes a new instance of the BeeYardHiveLocation class.
        /// </summary>
        public BeeYardHiveLocation()
        {
        }

        /// <summary>
        /// Gets or sets the id of the resource for this location - used for serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
