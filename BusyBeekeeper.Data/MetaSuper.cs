using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data
{
    public sealed class MetaSuper
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int PurchasePrice { get; set; }

        public int Depth { get; set; }
    }
}
