using System;

namespace BusyBeekeeper
{
    /// <summary>
    /// This core class manages the current player and provides methods for
    /// saving, loading, and enumerating the various game slots.
    /// </summary>
    /// <remarks>
    /// This method makes heavy use of the TopSlotKey, MiddleSlotKey, and BottomSlotKey values.
    /// This is due to a performance bottle-neck when working with a dictionary with enum key values
    /// due to the automatic boxing property when performing enum comparisons.
    /// </remarks>
    public class PlayerManager
    {
        /// <summary>
        /// Initializes a new instance of the PlayerManager class.
        /// </summary>
        public PlayerManager()
        {
        }

        /// <summary>
        /// Gets the key value for referencing the top slot.
        /// </summary>
        public const int TopSlotKey = 0;

        /// <summary>
        /// Gets the key value for referencing the middle slot.
        /// </summary>
        public const int MiddleSlotKey = 1;

        /// <summary>
        /// Gets the key value for referencing the bottom slot.
        /// </summary>
        public const int BottomSlotKey = 2;

        /// <summary>
        /// Gets the currently loaded player.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Saves the currently loaded Player.
        /// </summary>
        public void SavePlayer()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new Player at the given slot key. The new Player
        /// will become the current Player.
        /// </summary>
        /// <param name="slotKey"></param>
        /// <returns>The created Player</returns>
        public Player CreatePlayer(int slotKey)
        {
            this.ValidateKey(slotKey, "slotKey");

            // TODO: this.Player = new Player { };

            return this.Player;
        }

        /// <summary>
        /// Loads the Player using the given slot key.
        /// </summary>
        /// <param name="slotKey">The slotkey to load.</param>
        /// <returns>The loaded Player, this will be the same as the Player property.</returns>
        public Player LoadPlayer(int slotKey)
        {
            this.ValidateKey(slotKey, "slotKey");

            // TODO: this.Player = ... something

            return this.Player;
        }

        public PlayerSummary LoadSummary(int slotKey)
        {
            this.ValidateKey(slotKey, "slotKey");

            // TODO: This is the simplest return we can do for now.
            return new PlayerSummary { PlayerExists = false };
        }

        /// <summary>
        /// Validates the given slot key, throwing an agurment exception if
        /// it is no valid.
        /// </summary>
        /// <param name="slotKey">The slot key to check.</param>
        /// <param name="paramName">The name of the argument to throw.</param>
        private void ValidateKey(int slotKey, string paramName)
        {
            if ((slotKey != TopSlotKey) &&
                (slotKey != MiddleSlotKey) &&
                (slotKey != BottomSlotKey))
            {
                throw new ArgumentException(paramName);
            }
        }
    }
}
