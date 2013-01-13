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

        private readonly List<IUpdatable> mUpdatables = new List<IUpdatable>();
        private readonly List<BeeYardManager> mBeeYardManagers = new List<BeeYardManager>();
        
        public void CreateNew(
            int id, string name, PlayerAvatar avatar, BeeWorldManager beeWorldManager,
            IObjectRepository<BeeYard> beeYardRepository,
            IMetaObjectRepository<MetaSuper, Super> superRepository,
            IObjectRepository<LawnMower> lawnMowerRepository)
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
            var lInitialSuper = superRepository.CreateObject(0);
            lFirstBeeYard.BeeHives[0].Supers.Add(lInitialSuper);

            beeWorldManager.Time.Reset(this.mPlayer.BeeTime);

            this.mBeeYardManagers.Clear();
            this.mBeeYardManagers.AddRange(this.mPlayer.BeeYards.Select(x => new BeeYardManager(x)));

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

            foreach (var lUpdatable in this.mUpdatables)
            {
                lUpdatable.UpdateTick(worldManager);
            }
        }
    }
}
