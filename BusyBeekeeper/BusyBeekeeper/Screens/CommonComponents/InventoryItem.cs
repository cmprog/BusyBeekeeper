using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens.CommonComponents
{
    /// <summary>
    /// Represents a single inventory item.
    /// </summary>
    internal sealed class InventoryItem
    {
        public object Tag { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public Texture2D Texture { get; set; }
    }
}
