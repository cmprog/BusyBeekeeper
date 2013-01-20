using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// The basic hosing unit of a bee colony. A bee hive lives at a yard and is made up
    /// of various supers.
    /// </summary>
    public class BeeHive
    {
        private readonly IList<Super> mSupers = new List<Super>();

        public int Id { get; set; }
        
        public QueenBee QueenBee { get; set; }

        public int Population { get; set; }
        public int HoneyCollected { get; set; }
        public int ColonyStrength { get; set; }
        public int ColonyAgressiveness { get; set; }
        public int ColonySwarmLikeliness { get; set; }

        public IList<Super> Supers
        {
            get { return this.mSupers; }
        }
    }
}
