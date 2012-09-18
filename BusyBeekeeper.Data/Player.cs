using System;
using System.Collections.Generic;
using System.Linq;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// The player, the golden entity in the application, without the player, there is nothing.
    /// </summary>
    public class Player : Component
    {
        /// <summary>
        /// Initializes a new instance of the Player class.
        /// </summary>
        /// <param name="name">The name of the new player.</param>
        public Player(int id, string name, PlayerAvatar avatar)
        {
            this.Name = string.IsNullOrWhiteSpace(name) ? "Bob" : name;
            this.Avatar = avatar;
            this.AvailableCoins = NotifyingSharedProperty.Create(this.MessageDispatcher, "AvailableCoins", 0);
            this.TotalCoinsEarned = NotifyingSharedProperty.Create(this.MessageDispatcher, "TotalCoinsEarned", 0);
            this.TotalCoinsSpent = NotifyingSharedProperty.Create(this.MessageDispatcher, "TotalCoinsSpent", 0);
            this.Awards = new ComponentCollection<Award>(this.MessageDispatcher, "Awards");
            this.UnlockedBeeYards = new ComponentCollection<BeeYard>(this.MessageDispatcher, "UnlockedBeeYards");
            this.FilledBottles = new ComponentCollection<Bottle>(this.MessageDispatcher, "FilledBottles");
            this.EmptyBottles = new ComponentCollection<Bottle>(this.MessageDispatcher, "EmptyBottles");
            this.QueenBees = new ComponentCollection<QueenBee>(this.MessageDispatcher, "QueenBees");
            this.Supers = new ComponentCollection<Super>(this.MessageDispatcher, "Supers");
            this.SuperPaints = new ComponentCollection<SuperPaint>(this.MessageDispatcher, "SuperPaints");
        }

        /// <summary>
        /// Gets the ID of the player, this cooresponds to which save slot the player
        /// belongs to.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the name of the BeeKeeper.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the amount of real world time the player has been playing this character.
        /// </summary>
        public NotifyingSharedProperty<TimeSpan> TotalRealTimePlayed { get; set; }

        /// <summary>
        /// Gets the number of available coints the player has to spend.
        /// </summary>
        public NotifyingSharedProperty<int> AvailableCoins { get; set; }

        /// <summary>
        /// Gets the total number of coins earned by the player in the course of their
        /// entire gameplay.
        /// </summary>
        public NotifyingSharedProperty<int> TotalCoinsEarned { get; set; }

        /// <summary>
        /// Gets the total number of coins spent by the player in the course
        /// of their entiry gameplay.
        /// </summary>
        public NotifyingSharedProperty<int> TotalCoinsSpent { get; set; }

        /// <summary>
        /// Gets the bee suit owned by the player.
        /// </summary>
        public BeeSuit BeeSuit { get; set; }

        /// <summary>
        /// Gets or sets the BeeYard the player is currently located at.
        /// </summary>
        public BeeYard BeeYard { get; set; }

        /// <summary>
        /// Gets or sets the BeeHive the player is currently located at.
        /// </summary>
        public BeeHive BeeHive { get; set; }

        /// <summary>
        /// Gets or sets the HoneyExtractor owned by the player.
        /// </summary>
        public HoneyExtractor HoneyExtractor { get; set; }

        /// <summary>
        /// Gets or sets the LawnMower owned by the player.
        /// </summary>
        public LawnMower LawnMower { get; set; }

        /// <summary>
        /// Gets or sets the MarketBooth owned by the player.
        /// </summary>
        public MarketBooth MarketBooth { get; set; }

        /// <summary>
        /// Gets or sets the primary MarketHelper hired by the player.
        /// </summary>
        public MarketHelper PrimaryMarketHelper { get; set; }

        /// <summary>
        /// Gets or sets the secondary MarketHelper hired by the player.
        /// </summary>
        public MarketHelper SecondaryMarketHelper { get; set; }

        /// <summary>
        /// Gets or sets the PlayerAvatar which represents the player.
        /// </summary>
        public PlayerAvatar Avatar { get; set; }

        /// <summary>
        /// Gets or sets the Smoker owned by the player.
        /// </summary>
        public Smoker Smoker { get; set; }

        /// <summary>
        /// Gets or sets the Truck owned by the player.
        /// </summary>
        public Truck Truck { get; set; }

        /// <summary>
        /// Gets or sets the UncapingKnife owned by the player.
        /// </summary>
        public UncapingKnife UncapingKnife { get; set; }

        /// <summary>
        /// Gets a collection of Awards which the player has earned.
        /// </summary>
        public ComponentCollection<Award> Awards { get; private set; }

        /// <summary>
        /// Gets a collection of BeeYards which are owned by the player.
        /// </summary>
        public ComponentCollection<BeeYard> UnlockedBeeYards { get; private set; }

        /// <summary>
        /// Gets a collection of Bottles which have already been filled by the player.
        /// </summary>
        public ComponentCollection<Bottle> FilledBottles { get; private set; }

        /// <summary>
        /// Gets a collection of Bottles which have not yet been filled by the player.
        /// </summary>
        public ComponentCollection<Bottle> EmptyBottles { get; private set; }

        /// <summary>
        /// Gets a collection of the queen bees which have not yet been added to
        /// a bee hive.
        /// </summary>
        public ComponentCollection<QueenBee> QueenBees { get; private set; }

        /// <summary>
        /// Gets a collection of supers which the player has not yet added to a
        /// bee yard.
        /// </summary>
        public ComponentCollection<Super> Supers { get; private set; }

        /// <summary>
        /// Gets a collection of paint which has yet to be used to paint a super.
        /// </summary>
        public ComponentCollection<SuperPaint> SuperPaints { get; private set; }
    }
}
