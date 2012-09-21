using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data.Serialization
{
    /// <summary>
    /// Represents a BeeYard which can be easily serialized and deserialized.
    /// </summary>
    internal class SerializableBeeYard
    {
        /// <summary>
        /// Creates a new SerializableBeeYard from the given BeeYard.
        /// </summary>
        /// <param name="player">The BeeYard to convert.</param>
        /// <returns>A new SerializableBeeYard representing the given BeeYard.</returns>
        public static SerializableBeeYard FromBeeYard(BeeYard beeYard)
        {
            return new SerializableBeeYard
            {
            };
        }
    }
}
