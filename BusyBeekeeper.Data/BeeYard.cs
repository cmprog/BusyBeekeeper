using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// A bee yard is a location where bee hives can be managed. Different yards have various pros, cons, and 
    /// capacities. A bee yard is defines not just by the resource information which controls various danger
    /// and productivity factories, but also by the bee hives which the player has created at the yard.
    /// </summary>
    public class BeeYard : Component
    {
        /// <summary>
        /// Initializes a new instance of the BeeYard class.
        /// </summary>
        public BeeYard()
        {
        }

        /// <summary>
        /// Gets the resource ID of this yard - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }

        public SharedProperty<int> DangerFactor { get; set; }
        public SharedProperty<int> RegrowthFactor { get; set; }
        public SharedProperty<int> ProductivityFactor { get; set; }

        public int MaxHiveCount { get; set; }
        public int MaxHiveHeight { get; set; }

        /// <summary>
        /// Gets a collection of BeeYards which are owned by the player.
        /// </summary>
        public ComponentCollection<BeeHive> BeeHives { get; private set; }
    }
}
