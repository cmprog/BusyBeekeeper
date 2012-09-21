using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// A bottle holds the extracted honey which can be sold at the farmer's market.
    /// </summary>
    public class Bottle : Component
    {
        /// <summary>
        /// Initializes a new instance of the Bottle class.
        /// </summary>
        public Bottle()
        {
        }

        /// <summary>
        /// Gets the resource ID of this bottle - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
