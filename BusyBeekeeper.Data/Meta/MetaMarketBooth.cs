using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data.Meta
{
    /// <summary>
    /// Defines the persisted resource information about what makes a market booth.
    /// </summary>
    public class MetaMarketBooth
    {
        /// <summary>
        /// Gets or sets the unique ID associated with this booth.
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
        /// Gets or sets a factor which influences the amount of customers
        /// per market which may pass by the booth.
        /// </summary>
        public float PopularityFactor { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences a customer's willingness
        /// to actually try to buy honey.
        /// </summary>
        public float CustomerWillingnessToBuyFactor { get; set; }
    }
}
