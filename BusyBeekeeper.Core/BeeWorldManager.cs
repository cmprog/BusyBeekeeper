using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Data;

namespace BusyBeekeeper.Core
{
    public sealed class BeeWorldManager
    {
        private readonly Random mRandom;
        private readonly BeeTime mTime;
        private readonly BeeTimeSpan mElapsedTimeSpan;

        private readonly PlayerManager mPlayerManager;
        private readonly List<IUpdatable> mUpdatables = new List<IUpdatable>();
        private readonly List<BeeYardManager> mBeeYardManagers = new List<BeeYardManager>();

        private TimeSpan mLastTick;

        public BeeWorldManager()
        {
            this.mRandom = new Random();
            this.mTime = new BeeTime();
            this.mElapsedTimeSpan = new BeeTimeSpan();
            this.mPlayerManager = new PlayerManager(this);
            this.ResetTimeRates();
        }

        public Random Random
        {
            get { return this.mRandom; }
        }

        public PlayerManager PlayerManager
        {
            get { return this.mPlayerManager; }
        }

        public BeeTime Time 
        {
            get { return this.mTime; }
        }

        public BeeTimeSpan ElapsedTime
        {
            get { return this.mElapsedTimeSpan; }
        }

        /// <summary>
        /// Gets the number of real-world seconds between ticks.
        /// </summary>
        public TimeSpan RealTimePerTick { get; set; }

        /// <summary>
        /// Gets the number of minutes (in the bee world) to move forward 
        /// every tick.
        /// </summary>
        public int BeeMinutesPerTick { get; set; }

        public bool IsPaused { get; set; }

        /// <summary>
        /// Resets the SecondsPerTick / BeeMinutesPerTick to default values.
        /// </summary>
        public void ResetTimeRates()
        {
            const int lcDefaultMillisecondsPerTick = 1000;
            const int lcDefaultBeeMinutesPerTick = 2;

            this.RealTimePerTick = TimeSpan.FromMilliseconds(lcDefaultMillisecondsPerTick);
            this.BeeMinutesPerTick = lcDefaultBeeMinutesPerTick;
        }
                
        public void Update(TimeSpan totalGameTime)
        {
            if (this.mLastTick == TimeSpan.Zero)
            {
                this.mLastTick = totalGameTime;
                return;
            }

            var lElapsedGameTime = totalGameTime - this.mLastTick;
            if (lElapsedGameTime > this.RealTimePerTick)
            {
                this.mLastTick = totalGameTime;
                if (this.IsPaused) return;

                this.PlayerManager.Update(this.RealTimePerTick);

                var lMinutes = this.Time.Minute + this.BeeMinutesPerTick;
                var lHours = this.Time.Hour;
                var lDays = this.Time.Day;

                var lHasHourChanged = false;
                var lHasDayChanged = false;

                if (lMinutes >= BeeTime.MinutesInHour)
                {
                    lMinutes -= BeeTime.MinutesInHour;
                    lHasHourChanged = true;
                    lHours++;
                }

                if (lHours >= BeeTime.HoursInDay)
                {
                    lHours -= BeeTime.HoursInDay;
                    lHasDayChanged = true;
                    lDays++;
                }

                this.ElapsedTime.TotalMinutes = this.BeeMinutesPerTick;
                this.ElapsedTime.HasDayChanged = lHasDayChanged;
                this.ElapsedTime.HasHourChanged = lHasHourChanged;

                this.Time.Day = lDays;
                this.Time.Hour = lHours;
                this.Time.Minute = lMinutes;

                this.PlayerManager.UpdateTick(this);
            }
        }
    }
}
