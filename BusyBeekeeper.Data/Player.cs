using System;
using System.Collections.Generic;
using System.Linq;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// The player, the golden entity in the application, without the player, there is nothing.
    /// </summary>
    public sealed class Player
    {
        private readonly IList<Award> mAwards = new List<Award>();
        private readonly IList<BeeYard> mBeeYards = new List<BeeYard>();
        private readonly IList<Bottle> mBottles = new List<Bottle>();
        private readonly IList<QueenBee> mQueenBees = new List<QueenBee>();
        private readonly IList<Super> mSupers = new List<Super>();
        private readonly IList<SuperPaint> mSuperPaints = new List<SuperPaint>();

        public int Id { get; set; }

        public string Name { get; set; }
        public PlayerAvatar Avatar { get; set; }
        
        public DateTime CreationTime { get; set; }
        public DateTime LastPlayed { get; set; }
        public DateTime LastSaved { get; set; }
        public TimeSpan TotalRealTimePlayed { get; set; }
        public BeeTime BeeTime { get; set; }

        public int AvailableCoins { get; set; }
        public int TotalCoinsEarned { get; set; }

        public BeeSuit BeeSuit { get; set; }
        public Smoker Smoker { get; set; }
        public UncapingKnife UncapingKnife { get; set; }

        public Truck Truck { get; set; }
        public HoneyExtractor HoneyExtractor { get; set; }

        public bool IsTraveling { get; set; }
        public PlayerLocation Location { get; set; }
        public BeeYard CurrentBeeYard { get; set; }
        public BeeHive CurrentBeeHive { get; set; }

        public IList<Award> Awards
        { 
            get { return this.mAwards; } 
        }

        public IList<BeeYard> BeeYards
        {
            get { return this.mBeeYards; }
        }

        private IList<Bottle> Bottles
        {
            get { return this.mBottles; }
        }

        public IList<QueenBee> QueenBees
        {
            get { return this.mQueenBees; }
        }

        public IList<Super> Supers
        {
            get { return this.mSupers; }
        }

        public IList<SuperPaint> SuperPaints
        {
            get { return this.mSuperPaints; }
        }

        #region Instance Methods


        public void TravelTo(BeeYard beeYard)
        {
            if (beeYard == this.CurrentBeeYard)
            {
                return;
            }

            this.IsTraveling = true;
            this.CurrentBeeYard = beeYard;
            this.CurrentBeeHive = null;
            this.Location = PlayerLocation.BeeYard;
        }

        public void TravelTo(BeeHive beeHive)
        {
            if (this.CurrentBeeYard.BeeHives.Contains(beeHive))
            {
                this.CurrentBeeHive = beeHive;
                this.Location = PlayerLocation.BeeHive;
            }
            else
            {
                this.IsTraveling = true;
                this.CurrentBeeYard = this.BeeYards.First(x => x.BeeHives.Contains(beeHive));
                this.CurrentBeeHive = beeHive;
                this.Location = PlayerLocation.BeeHive;
            }
        }

        public void TravelToHoneyHouse()
        {
            this.IsTraveling = true;
            this.CurrentBeeHive = null;
            this.CurrentBeeYard = null;
            this.Location = PlayerLocation.HoneyHouse;
        }

        #endregion -- Instance Methods
    }
}
