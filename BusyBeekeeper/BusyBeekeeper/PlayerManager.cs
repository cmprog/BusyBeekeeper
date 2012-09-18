using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BusyBeekeeper.Data;

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
            var lFilePath = GetSaveFilePath(this.Player.Id);
            using (var lTextWriter = new StreamWriter(lFilePath))
            {
                var lSerializer = new XmlSerializer(typeof(Player));
                lSerializer.Serialize(lTextWriter, this.Player);
            }
        }

        /// <summary>
        /// Creates a new Player at the given slot key. The new Player
        /// will become the current Player.
        /// </summary>
        /// <param name="slotKey"></param>
        /// <returns>The created Player</returns>
        public Player CreatePlayer(int slotKey, string playerName, PlayerAvatar avatar)
        {
            this.ValidateKey(slotKey, "slotKey");
            this.Player = new Player(slotKey, playerName, avatar);
            this.SavePlayer();
            return this.Player;
        }

        public PlayerSummary LoadSummary(int slotKey)
        {
            this.ValidateKey(slotKey, "slotKey");
            
            Player player;
            if (this.TryLoadPlayer(slotKey, out player))
            {
                return new PlayerSummary
                {
                    PlayerExists = true,
                    AwardCount = player.Awards.Count,
                    BeeKeeperName = player.Name,
                    SlotKey = slotKey,
                    TotalTimePlayed = player.TotalRealTimePlayed.Value,
                    AvatarTexturePath = "Sprites/Blank",
                };
            }
            else
            {
                return new PlayerSummary
                { 
                    PlayerExists = false,
                    SlotKey = slotKey,
                };
            }
        }

        /// <summary>
        /// Loads the Player using the given slot key.
        /// </summary>
        /// <param name="slotKey">The slotkey to load.</param>
        /// <returns>The loaded Player, this will be the same as the Player property.</returns>
        public Player LoadPlayer(int slotKey)
        {
            this.ValidateKey(slotKey, "slotKey");

            Player player;
            if (!this.TryLoadPlayer(slotKey, out player))
            {
                System.Diagnostics.Debug.WriteLine("Attempting to load a player which does not exist.");
            }

            return this.Player;
        }

        /// <summary>
        /// Tries to load the given player.
        /// </summary>
        /// <param name="slotKey">The slot to load.</param>
        /// <param name="player">The player which will be loaded.</param>
        /// <returns>True if the player was loaded, false otherwise.</returns>
        private bool TryLoadPlayer(int slotKey, out Player player)
        {
            this.ValidateKey(slotKey, "slotKey");
            player = null;

            var lFilePath = GetSaveFilePath(slotKey);

            if (File.Exists(lFilePath))
            {
                try
                {
                    using (var lTextReader = new StreamReader(lFilePath))
                    {
                        var lSerializer = new XmlSerializer(typeof(Player));
                        player = (Player)lSerializer.Deserialize(lTextReader);
                        return true;
                    }
                }
                catch (Exception lPokemonException)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to load player file: " + lFilePath);
                    System.Diagnostics.Debug.WriteLine("===== Exception Message ==============================================");
                    System.Diagnostics.Debug.WriteLine(lPokemonException);
                    System.Diagnostics.Debug.WriteLine("======================================================================");

                    return false;
                }
            }
            else return false;
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

        /// <summary>
        /// Gets the absolute file path for the given filename.
        /// </summary>
        /// <param name="slotId">The slot ID to get the save file for.</param>
        /// <returns>The absolute file path for the save file in the given slot id.</returns>
        private static string GetSaveFilePath(int slotId)
        {
            var lApplicationDataDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var lBeeFreeDirectoryPath = Path.Combine(lApplicationDataDirectoryPath, "BusyBeekeeper");
            if (!Directory.Exists(lBeeFreeDirectoryPath))
            {
                Directory.CreateDirectory(lBeeFreeDirectoryPath);
            }

            var lFilePath = Path.Combine(lBeeFreeDirectoryPath, "beekeeper_" + slotId + ".bkpr");
            return lFilePath;
        }
    }
}
