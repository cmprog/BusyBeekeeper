using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data.Meta
{
    /// <summary>
    /// Defines the persisted resource information about what makes a BeeSuit.
    /// </summary>
    public class MetaBeeSuit
    {
        /// <summary>
        /// Gets or sets the unique ID associated with this BeeSuit.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cost of this bee suit.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets or sets the level of this bee suit.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the path of the texture to use in the shop.
        /// </summary>
        public int ShopTexturePath { get; set; }

        /// <summary>
        /// Gets or sets the name of this bee suit.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a description for this bee suit.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the strength of this bee suit.
        /// </summary>
        public int Strength { get; set; }

        public BeeSuit ToBeeSuit()
        {
            return null;
        }
    }
}
