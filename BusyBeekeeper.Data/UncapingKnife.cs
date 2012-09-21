using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Uncapping knifes are used to uncap the frames from a harvested super.
    /// </summary>
    public class UncapingKnife : Component
    {
        /// <summary>
        /// Initializes a new instance of the UncapingKnife class.
        /// </summary>
        public UncapingKnife()
        {
        }

        /// <summary>
        /// Gets the resource ID of this knife - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
