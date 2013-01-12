using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data
{
    public sealed class BeeTime
    {
        public const int MinutesInHour = 60;
        public const int HoursInDay = 12;

        public int Minute { get; set; }
        public int Hour { get; set; }
        public int Day { get; set; }

        public void Reset(BeeTime beeTime)
        {
            this.Minute = beeTime.Minute;
            this.Hour = beeTime.Hour;
            this.Day = beeTime.Day;
        }

        public override string ToString()
        {
            return string.Concat(this.Day, ":", this.Hour, ":", this.Minute);
        }
    }
}
