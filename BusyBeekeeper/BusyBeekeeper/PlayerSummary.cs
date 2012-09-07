using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper
{
    /// <summary>
    /// Contains a summary of the information about a single saved player.
    /// </summary>
    public class PlayerSummary
    {
        /// <summary>
        /// Gets or sets a flag indicating whether the player exists.
        /// </summary>
        public bool PlayerExists { get; set; }

        /// <summary>
        /// Gets the slot key associated with this player.
        /// </summary>
        public int SlotKey { get; set; }

        /// <summary>
        /// Gets or sets the player's Beekeeper's name.
        /// </summary>
        public string BeeKeeperName { get; set; }

        /// <summary>
        /// Gets or sets the total time the player has played.
        /// </summary>
        public TimeSpan TotalTimePlayed { get; set; }

        /// <summary>
        /// Gets or sets the total number of awards for this player.
        /// </summary>
        public int AwardCount { get; set; }

        /// <summary>
        /// Gets or sets the texture path for the player's avatar.
        /// </summary>
        public string AvatarTexturePath { get; set; }
    }
}
