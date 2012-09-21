using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// A honey extractor extracts honey from the frames extracted from a super.
    /// </summary>
    public class HoneyExtractor : Component
    {
        /// <summary>
        /// Initializes a new instance of the HoneyExtractor class.
        /// </summary>
        public HoneyExtractor()
        {
        }

        /// <summary>
        /// Gets the resource ID of this extractor - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
