using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Defines the resource information for the various possible avatars a player can have.
    /// </summary>
    public class PlayerAvatar : Component
    {
        /// <summary>
        /// Initializes a new instance of the PlayerAvatar class.
        /// </summary>
        public PlayerAvatar()
        {
        }

        /// <summary>
        /// Gets the resource ID of this avatar - used in serialization.
        /// </summary>
        public int ResourceId { get; set; }
    }
}
