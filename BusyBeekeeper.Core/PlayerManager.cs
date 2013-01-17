using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Data;

namespace BusyBeekeeper.Core
{
    public sealed class PlayerManager : IUpdatable
    {
        private Player mPlayer;

        private readonly BeeWorldManager mWorldManager;
        private readonly List<IUpdatable> mUpdatables = new List<IUpdatable>();
        private readonly List<BeeYardManager> mBeeYardManagers = new List<BeeYardManager>();

        public PlayerManager(BeeWorldManager worldManager)
        {
            this.mWorldManager = worldManager;
        }

        private bool mIsTraveling;
        private int mTravelTicksRemaining;
        private Action mTravelCompleteCallback;
        
        public void CreateNew(
            int id, string name, PlayerAvatar avatar,
            IObjectRepository<BeeYard> beeYardRepository,
            IMetaObjectRepository<MetaSuper, Super> superRepository,
            IObjectRepository<LawnMower> lawnMowerRepository,
            IMetaObjectRepository<MetaQueenBee, QueenBee> queenRepository,
            IObjectRepository<Smoker> smokerRepository)
        {
            this.mPlayer = new Player();
            this.mPlayer.Id = id;
            this.mPlayer.Name = name;
            this.mPlayer.Avatar = avatar;
            this.mPlayer.CreationTime = DateTime.Now;
            this.mPlayer.LastPlayed = DateTime.Now;
            this.mPlayer.TotalRealTimePlayed = TimeSpan.Zero;
            this.mPlayer.BeeTime = new BeeTime();
            this.mPlayer.LawnMower = lawnMowerRepository.CreateObject(0);
            this.mPlayer.Smoker = smokerRepository.CreateObject(0);

            for (int lBeeYardId = 0; lBeeYardId < beeYardRepository.Count; lBeeYardId++)
            {
                var lBeeYard = beeYardRepository.CreateObject(lBeeYardId);
                this.mPlayer.BeeYards.Insert(lBeeYardId, lBeeYard);

                for (int lHiveIndex = 0; lHiveIndex <= lBeeYard.MaxHiveCount; lHiveIndex++)
                {
                    var lBeeHive = new BeeHive();
                    lBeeHive.Id = lHiveIndex;
                    lBeeYard.BeeHives.Insert(lHiveIndex, lBeeHive);
                }
            }

            var lFirstBeeYard = this.mPlayer.BeeYards[0];
            lFirstBeeYard.IsUnlocked = true;
            
            // TEST:
            this.mPlayer.BeeYards[1].IsUnlocked = true;
            foreach (var lSuper in Enumerable.Repeat(superRepository.CreateObject(0), 20)) this.mPlayer.Supers.Add(lSuper);
            // -- End TEST

            var lFirstBeeHive = lFirstBeeYard.BeeHives[0];
            lFirstBeeHive.Supers.Add(superRepository.CreateObject(0));
            lFirstBeeHive.QueenBee = queenRepository.CreateObject(0);

            this.mWorldManager.Time.Reset(this.mPlayer.BeeTime);

            this.mBeeYardManagers.Clear();
            this.mBeeYardManagers.AddRange(
                this.mPlayer.BeeYards
                    .Select(x => new BeeYardManager(this.mWorldManager, x)));

            this.mUpdatables.Clear();
            this.mUpdatables.AddRange(this.mBeeYardManagers);
        }

        public void Load(int id, BeeWorldManager beeWorldManager)
        {
            throw new NotImplementedException();
        }

        public Player Player
        {
            get { return this.mPlayer; }
        }

        public bool IsTraveling
        {
            get { return this.mIsTraveling; }
        }

        public IList<BeeYardManager> BeeYardManagers
        {
            get { return this.mBeeYardManagers; }
        }

        public void Save()
        {
            if (this.mPlayer == null) return;
            // TODO:
        }

        internal void Update(TimeSpan elapsedRealTime)
        {
            if (this.mPlayer != null)
            {
                this.mPlayer.TotalRealTimePlayed =
                   this.mPlayer.TotalRealTimePlayed.Add(elapsedRealTime);
            }
        }

        public void UpdateTick(BeeWorldManager worldManager)
        {
            if (this.mPlayer == null) return;

            if (this.IsTraveling && (--this.mTravelTicksRemaining == 0))
            {
                this.mIsTraveling = false;
                this.mWorldManager.ResetTimeRates();
                this.mTravelCompleteCallback();
            }

            foreach (var lUpdatable in this.mUpdatables)
            {
                lUpdatable.UpdateTick(worldManager);
            }
        }

        /// <summary>
        /// Travels back to the bee yard of the current bee hive. This method
        /// should only be called when at a bee hive.
        /// </summary>
        public void TravelToBeeYard()
        {
            System.Diagnostics.Debug.Assert(this.mPlayer.Location == PlayerLocation.BeeHive);

            this.mPlayer.CurrentBeeHive = null;
            this.mPlayer.Location = PlayerLocation.BeeYard;
        }

        /// <summary>
        /// Travels to the given bee yard, calling the given callback action when traveling
        /// is complete.
        /// </summary>
        /// <param name="beeYard">The bee yard to travel to.</param>
        /// <param name="callback">The callback method to call after traveling is complete.</param>
        public void TravelTo(BeeYard beeYard, Action callback)
        {
            System.Diagnostics.Debug.Assert(beeYard != this.mPlayer.CurrentBeeYard);

            this.mPlayer.CurrentBeeYard = beeYard;
            this.mPlayer.CurrentBeeHive = null;
            this.mPlayer.Location = PlayerLocation.BeeYard;

            this.StartTraveling(callback);
        }

        /// <summary>
        /// Travels to the given bee hive in the current bee yard. This method should
        /// only be called when the player is already at a bee yard. Traveling can only occur
        /// to a bee hive in the current bee yard.
        /// </summary>
        /// <param name="beeHive"></param>
        public void TravelTo(BeeHive beeHive)
        {
            System.Diagnostics.Debug.Assert(this.mPlayer.Location == PlayerLocation.BeeYard);
            System.Diagnostics.Debug.Assert(this.mPlayer.CurrentBeeYard.BeeHives.Contains(beeHive));

            this.mPlayer.CurrentBeeHive = beeHive;
            this.mPlayer.Location = PlayerLocation.BeeHive;
        }

        /// <summary>
        /// Travels to the honey house. This method can be called anywhere as long as the player
        /// is not already at the honeyhouse. The callback method provided is called when
        /// traveling is complete.
        /// </summary>
        /// <param name="callback">The callback to call when traveling is complete.</param>
        public void TravelToHoneyHouse(Action callback)
        {
            System.Diagnostics.Debug.Assert(this.mPlayer.Location != PlayerLocation.HoneyHouse);

            this.mPlayer.CurrentBeeHive = null;
            this.mPlayer.CurrentBeeYard = null;
            this.mPlayer.Location = PlayerLocation.HoneyHouse;

            this.StartTraveling(callback);
        }

        /// <summary>
        /// Starts the traveling process.
        /// </summary>
        /// <param name="callback">The callback method to call when traveling is complete.</param>
        private void StartTraveling(Action callback)
        {
            // TODO: Travel ticks should be based on the player's truck speed.
            this.mWorldManager.RealTimePerTick = TimeSpan.FromMilliseconds(300);
            this.mTravelCompleteCallback = callback;
            this.mTravelTicksRemaining = 5;
            this.mIsTraveling = true;
        }
    }
}
