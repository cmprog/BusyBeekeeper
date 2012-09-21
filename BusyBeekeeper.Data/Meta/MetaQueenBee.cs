using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data.Meta
{
    /// <summary>
    /// Defines the persited resource information about what makes a queen bee.
    /// </summary>
    public class MetaQueenBee
    {
        /// <summary>
        /// Gets or sets the unique ID associated with this bottle.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the bottle.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a description of the bottle.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the cost of the bottle.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets or sets the path to the texture when purchasing this bottle
        /// in the store.
        /// </summary>
        public string ShopTexturePath { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the rate at which a bee
        /// hive's population grows.
        /// </summary>
        public float BeePopulationGrowthFactor { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the rate which honey is
        /// collected (by the bees).
        /// </summary>
        public float HoneyCollectionFactor { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the overall strength of
        /// the colony.
        /// </summary>
        public float ColonyStrengthFactor { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the normal state of aggression
        /// of the colony.
        /// </summary>
        public float NaturalBeeAgressionFactor { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the likeliness for a colony
        /// to swarm over time.
        /// </summary>
        public float SwarmLikelinessFactor { get; set; }
    }
}
