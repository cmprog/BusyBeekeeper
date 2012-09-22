using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data.Meta
{
    /// <summary>
    /// Defines the persisted resource information about what makes an uncaping knife.
    /// </summary>
    public class MetaUncapingKnife
    {
        /// <summary>
        /// Gets or sets the unique ID associated with this knife.
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
        /// Gets or sets the path to the texture when purchasing this truck
        /// in the store.
        /// </summary>
        public string ShopTexturePath { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences how much honey is
        /// lost in the process of uncapping.
        /// </summary>
        public float UncapingEfficiencyFactor { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the speed
        /// at which the player can uncap frames.
        /// </summary>
        public float UncapingSpeedFactor { get; set; }

        public UncapingKnife ToUncapingKnife()
        {
            return null;
        }
    }
}
