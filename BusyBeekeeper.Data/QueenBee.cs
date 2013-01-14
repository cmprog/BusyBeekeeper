using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data
{
    public sealed class QueenBee
    {
        public int MetaId { get; set; }
        public BeeTime PurchaseTime { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int BeePopulationGrowthFactor { get; set; }
        public int HoneyCollectionFactor { get; set; }
        public int ColonyStrengthFactor { get; set; }
        public int NaturalBeeAgressionFactor { get; set; }
        public int SwarmLikelinessFactor { get; set; }
    }
}
