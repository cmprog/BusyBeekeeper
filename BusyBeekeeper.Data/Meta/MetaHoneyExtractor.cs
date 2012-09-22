using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data.Meta
{
    /// <summary>
    /// Defines the persisted resource information about what makes a
    /// honey extractor.
    /// </summary>
    public class MetaHoneyExtractor
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
        /// Gets or sets the number of frames this extractor can hold.
        /// </summary>
        public int FrameCapcity { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the speed at which extraction occurs.
        /// </summary>
        public float ExtractionSpeedFactor { get; set; }

        public HoneyExtractor ToHoneyExtractor()
        {
            return null;
        }
    }
}
