using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Contains the various resource paths for the a market helper.
    /// </summary>
    public class MarketHelperAvatar : Component
    {
        /// <summary>
        /// Initializes a new instance of the MarketHelperAvatar class.
        /// </summary>
        public MarketHelperAvatar()
        {
        }

        /// <summary>
        /// Gets the resource ID of this avatar - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
