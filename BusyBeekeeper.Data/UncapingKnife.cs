using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Uncapping knifes are used to uncap the frames from a harvested super.
    /// </summary>
    public sealed class UncapingKnife
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }

        public int PurchasePrice { get; set; }
        public bool IsUnlocked { get; set; }

        public int UncapingEfficiencyFactor { get; set; }
        public int UncapingSpeedFactor { get; set; }
    }
}
