using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Defines the various properties of a helper the player can have helping them while
    /// working the farmer's market.
    /// </summary>
    public class MarketHelper : Component
    {
        /// <summary>
        /// Initializes a new instance of the MarketHelper class.
        /// </summary>
        public MarketHelper()
        {
        }

        /// <summary>
        /// Gets the resource ID of this helper - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
