using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data
{
    public sealed class Super
    {
        public int MetaId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int Depth { get; set; }

        public SuperPaint SuperPaint { get; set; }
    }
}
