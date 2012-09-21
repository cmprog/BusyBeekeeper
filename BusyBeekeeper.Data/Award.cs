using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// An award is a basic goal which awards the player with money when achieved.
    /// </summary>
    public class Award : Component
    {
        /// <summary>
        /// Initializes a new instance of the Award class.
        /// </summary>
        public Award()
        {
        }

        /// <summary>
        /// Gets the resource ID of this award - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
