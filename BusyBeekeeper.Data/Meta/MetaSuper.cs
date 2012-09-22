using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data.Meta
{
    /// <summary>
    /// Defines the persited resource information about what makes a super.
    /// </summary>
    public class MetaSuper
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
        /// Gets the depth of the super. This comes into play when stacking
        /// supers to make a hive, hives have a ceiling limit based on the
        /// yard they are part of.
        /// </summary>
        public float Depth { get; set; }
        
        public Super ToSuper()
        {
            return null;
        }
    }
}
