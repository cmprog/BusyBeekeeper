using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Defines the various properties for a queen bee. The queen bee influences various
    /// properties of the bee hive it is associated with.
    /// </summary>
    public class QueenBee : Component
    {
        /// <summary>
        /// Initializes a new instance of the QueenBee class.
        /// </summary>
        public QueenBee()
        {
        }

        /// <summary>
        /// Gets the resource ID of this queen bee - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
