using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// The basic hosing unit of a bee colony. A bee hive lives at a yard and is made up
    /// of various supers.
    /// </summary>
    public class BeeHive : Component
    {
        /// <summary>
        /// Initializes a new instance of the BeeHive class.
        /// </summary>
        public BeeHive()
        {
        }

        /// <summary>
        /// Gets the id of the location within the yard where this hive is located.
        /// </summary>
        public int YardLocationId { get; set; }
    }
}
