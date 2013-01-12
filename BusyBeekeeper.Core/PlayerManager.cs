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

        public void CreateNew(
            int id, string name, PlayerAvatar avatar, BeeWorldManager beeWorldManager,
            IObjectRepository<BeeYard> beeYardRepository)
        {
            this.mPlayer = new Player();
            this.mPlayer.Id = id;
            this.mPlayer.Name = name;
            this.mPlayer.Avatar = avatar;
            this.mPlayer.CreationTime = DateTime.Now;
            this.mPlayer.LastPlayed = DateTime.Now;
            this.mPlayer.TotalRealTimePlayed = TimeSpan.Zero;
            this.mPlayer.BeeTime = new BeeTime();

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

            beeWorldManager.Time.Reset(this.mPlayer.BeeTime);

            this.mUpdatables.Clear();
            this.mUpdatables.AddRange(this.mPlayer.BeeYards.Select(x => new BeeYardManager(x)));
        }

        public void Load(int id, BeeWorldManager beeWorldManager)
        {
            throw new NotImplementedException();
        }

        public Player Player
        {
            get { return this.mPlayer; }
        }

        public void Save()
        {
            throw new NotImplementedException();
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
