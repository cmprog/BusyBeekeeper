using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// An award is a basic goal which awards the player with money when achieved.
    /// </summary>
    public class Award
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasBeenEarned { get; set; }
        public bool AwardCoins { get; set; }
    }
}
