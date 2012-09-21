using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Smokers are used to help ease the aggression of a hive. They also help urge bees to return to the
    /// hive when transporting them.
    /// </summary>
    public class Smoker : Component
    {
        /// <summary>
        /// Initializes a new instance of the Smoker class.
        /// </summary>
        public Smoker()
        {
        }

        /// <summary>
        /// Gets the resource ID of this smoker - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
