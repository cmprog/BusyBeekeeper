using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data
{
    public sealed class BeeSuit
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public int IsUnlocked { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Strength { get; set; }
    }
}
