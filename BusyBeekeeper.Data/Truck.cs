using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Moving from one bee yard to another is a slow task, especially when transporting
    /// harvested supers. Trucks help out with that.
    /// </summary>
    public class Truck : Component
    {
        /// <summary>
        /// Initializes a new instance of the Truck class.
        /// </summary>
        public Truck()
        {
        }

        /// <summary>
        /// Gets the resource ID of this truck - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
