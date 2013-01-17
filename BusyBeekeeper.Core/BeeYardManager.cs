using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Data;

namespace BusyBeekeeper.Core
{
    public sealed class BeeYardManager : IUpdatable
    {
        private readonly BeeYard mBeeYard;

        private BeeWorldManager mBeeWorldManager;
        private LawnMower mLawnMower;
        private Action mLawnMowingCompleteCallback;

        private readonly List<IUpdatable> mUpdatables = new List<IUpdatable>();
        private readonly BeeHiveManager[] mBeeHiveManagers;

        const int sMaxGrassGrowth = 1000;
        const int sMinGrassGrowth = 0;

        public BeeYardManager(BeeWorldManager beeWorldManager, BeeYard beeYard)
        {
            if (beeWorldManager == null) throw new ArgumentNullException("beeWorldManager");
            if (beeYard == null) throw new ArgumentNullException("beeYard");

            this.mBeeWorldManager = beeWorldManager;
            this.mBeeYard = beeYard;

            this.mBeeHiveManagers = new BeeHiveManager[this.mBeeYard.MaxHiveCount];
            for (int lIndex = 0; lIndex < this.mBeeHiveManagers.Length; lIndex++)
            {
                var lHiveManager = new BeeHiveManager(this.mBeeWorldManager, this.mBeeYard, this.mBeeYard.BeeHives[lIndex]);
                this.mBeeHiveManagers[lIndex] = lHiveManager;
            }

            this.mUpdatables.AddRange(this.mBeeHiveManagers);
        }

        public bool IsMowingLawn { get; private set; }

        public IList<BeeHiveManager> BeeHiveManagers
        {
            get { return this.mBeeHiveManagers; }
        }

        public void UpdateTick(BeeWorldManager worldManager)
        {
            if (this.mBeeYard.IsUnlocked)
            {
                var lElapsedMinutes = worldManager.ElapsedTime.TotalMinutes;

                if (this.IsMowingLawn)
                {
                    this.mBeeYard.GrassGrowth =
                        Math.Max(sMinGrassGrowth, this.mBeeYard.GrassGrowth - (lElapsedMinutes * this.mLawnMower.SpeedFactor));

                    if (this.mBeeYard.GrassGrowth == sMinGrassGrowth)
                    {
                        this.IsMowingLawn = false;
                        this.mLawnMower = null;
                        this.mLawnMowingCompleteCallback();
                        this.mLawnMowingCompleteCallback = null;
                        this.mBeeWorldManager.ResetTimeRates();
                    }
                }
                else
                {
                    this.mBeeYard.GrassGrowth =
                        Math.Min(sMaxGrassGrowth, this.mBeeYard.GrassGrowth + (lElapsedMinutes * this.mBeeYard.RegrowthFactor));
                }
            }

            foreach (var lUpdatable in this.mUpdatables)
            {
                lUpdatable.UpdateTick(worldManager);
            }
        }

        /// <summary>
        /// Mows the lawn. This action will take some time.
        /// </summary>
        /// <param name="beeWorldManager"></param>
        /// <param name="lawnMower"></param>
        /// <param name="callback"></param>
        public void MowLawn(BeeWorldManager beeWorldManager, LawnMower lawnMower, Action callback)
        {
            System.Diagnostics.Debug.Assert(!this.IsMowingLawn);

            this.mBeeWorldManager.RealTimePerTick = TimeSpan.FromMilliseconds(300);
            this.mBeeWorldManager.BeeMinutesPerTick *= 2;
            this.mLawnMower = lawnMower;
            this.mLawnMowingCompleteCallback = callback;
            this.IsMowingLawn = true;
        }
    }
}
