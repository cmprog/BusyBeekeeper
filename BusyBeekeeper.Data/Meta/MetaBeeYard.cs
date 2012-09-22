using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BusyBeekeeper.Data.Meta
{
    /// <summary>
    /// Defines the persisted resource information about what makes a bee yard.
    /// </summary>
    public class MetaBeeYard
    {
        /// <summary>
        /// Gets or sets the unique ID associated with this bee yard.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of this yard.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the location on the map.
        /// </summary>
        public Vector2 MapLocation { get; set; }

        /// <summary>
        /// Gets or sets an array of vectors defining where
        /// each bee hive is located within the bee yard. The
        /// length of this array defines the number of bee hives
        /// which are allowed to exist at the yard at one time.
        /// </summary>
        public Vector2[] BeeHiveLocations { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the rate of growth of the
        /// grass at the bee yard.
        /// </summary>
        public float GrassGrowthFactor { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the rate which honey is
        /// collected (by the bees).
        /// </summary>
        public float HoneyCollectionFactor { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the rate which dangerous
        /// happenings occur at the yard.
        /// </summary>
        public float DangerFactor { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the rate at which a bee
        /// hive's population grows.
        /// </summary>
        public float BeePopulationGrowthFactor { get; set; }

        /// <summary>
        /// Gets or sets the maximum height each bee hive can achieve.
        /// </summary>
        public int BeeHiveCeiling { get; set; }

        public BeeYard ToBeeYard()
        {
            return null;
        }
    }
}
