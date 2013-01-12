using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data
{
    public sealed class BeeTimeSpan
    {
        public int TotalMinutes { get; set; }

        public bool HasHourChanged { get; set; }
        public bool HasDayChanged { get; set; }

        public override string ToString()
        {
            return string.Concat(this.TotalMinutes, "-", this.HasHourChanged, "-", this.HasDayChanged);
        }
    }
}
