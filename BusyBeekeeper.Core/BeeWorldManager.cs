using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Data;

namespace BusyBeekeeper.Core
{
    public sealed class BeeWorldManager
    {
        private readonly PlayerManager mPlayerManager;
        private readonly BeeTime mTime;
        private readonly BeeTimeSpan mElapsedTimeSpan;

        private TimeSpan mLastTick;

        public BeeWorldManager()
        {
            this.mTime = new BeeTime();
            this.mElapsedTimeSpan = new BeeTimeSpan();
            this.mPlayerManager = new PlayerManager();
        }

        public event Action<BeeWorldManager> Tick;

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

        public bool IsPaused { get; set; }
                
        public void Update(TimeSpan totalGameTime)
        {
            if (this.mLastTick == TimeSpan.Zero)
            {
                this.mLastTick = totalGameTime;
                return;
            }

            const int lcSecondPerTick = 6;
            const int lcGameMinutesPerTick = 10;

            var lElapsedGameTime = totalGameTime - this.mLastTick;
            if (lElapsedGameTime.TotalSeconds > lcSecondPerTick)
            {
                this.mLastTick = totalGameTime;
                if (this.IsPaused) return;

                this.PlayerManager.Update(TimeSpan.FromSeconds(lcSecondPerTick));

                var lMinutes = this.Time.Minute + lcGameMinutesPerTick;
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

                this.ElapsedTime.TotalMinutes = lcGameMinutesPerTick;
                this.ElapsedTime.HasDayChanged = lHasDayChanged;
                this.ElapsedTime.HasHourChanged = lHasHourChanged;

                this.Time.Day = lDays;
                this.Time.Hour = lHours;
                this.Time.Minute = lMinutes;

                this.PlayerManager.UpdateTick(this);

                var lTickHandler = this.Tick;
                if (lTickHandler != null)
                {
                    lTickHandler(this);
                }
            }
        }
    }
}
