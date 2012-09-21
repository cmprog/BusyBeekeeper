﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Defines the properties of a booth used at the farmer's market. Better booths provide better
    /// discoverability and greater inventory.
    /// </summary>
    public class MarketBooth : Component
    {
        /// <summary>
        /// Initializes a new instance of the MarketBooth class.
        /// </summary>
        public MarketBooth()
        {
        }

        /// <summary>
        /// Gets the resource ID of this booth - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
