using System.Collections.Generic;
using System.ComponentModel;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// A bee yard is a location where bee hives can be managed. Different yards have various pros, cons, and 
    /// capacities. A bee yard is defines not just by the resource information which controls various danger
    /// and productivity factories, but also by the bee hives which the player has created at the yard.
    /// </summary>
    public class BeeYard
    {
        private IList<BeeHive> mBeeHives = new List<BeeHive>();

        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int PurchasePrice { get; set; }
        public bool IsUnlocked { get; set; }

        public int DangerFactor { get; set; }
        public int RegrowthFactor { get; set; }
        public int ProductivityFactor { get; set; }

        public int MaxHiveCount { get; set; }
        public int MaxHiveHeight { get; set; }

        public IList<BeeHive> BeeHives
        {
            get { return this.mBeeHives; }
        }
    }
}
