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

        public BeeYardManager(BeeYard beeYard)
        {
            if (beeYard == null) throw new ArgumentNullException("beeYard");
            this.mBeeYard = beeYard;
        }

        public bool IsMowingLawn { get;private set; }

        public void UpdateTick(BeeWorldManager worldManager)
        {
            if (this.mBeeYard.IsUnlocked)
            {
                var lElapsedMinutes = worldManager.ElapsedTime.TotalMinutes;

                if (this.IsMowingLawn)
                {
                    this.mBeeYard.GrassGrowth =
                        Math.Max(0, this.mBeeYard.GrassGrowth - (lElapsedMinutes * this.mLawnMower.SpeedFactor));

                    if (this.mBeeYard.GrassGrowth == 0)
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
                    this.mBeeYard.GrassGrowth += lElapsedMinutes * this.mBeeYard.RegrowthFactor;
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

            this.mBeeWorldManager = beeWorldManager;
            this.mBeeWorldManager.SecondsPerTick = 1;
            this.mBeeWorldManager.BeeMinutesPerTick *= 2;
            this.mLawnMower = lawnMower;
            this.mLawnMowingCompleteCallback = callback;
            this.IsMowingLawn = true;
        }
    }
}
