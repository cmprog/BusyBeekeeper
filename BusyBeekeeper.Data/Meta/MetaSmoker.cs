using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data.Meta
{
    /// <summary>
    /// Defines the persited resource information about what makes smoker.
    /// </summary>
    public class MetaSmoker
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
        /// Gets or sets a factor which influences how much less aggresive the
        /// bees will become after smoking them.
        /// </summary>
        public float BeeAgressionFactor { get; set; }

        public Smoker ToSmoker()
        {
            return null;
        }
    }
}
