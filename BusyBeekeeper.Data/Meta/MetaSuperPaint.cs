using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BusyBeekeeper.Data.Meta
{
    /// <summary>
    /// Defines the persited resource information about what makes a super paint.
    /// </summary>
    public class MetaSuperPaint
    {
        /// <summary>
        /// Gets or sets the unique ID associated with this bottle.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the bottle.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a description of the bottle.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the cost of the bottle.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets or sets the path to the texture when purchasing this bottle
        /// in the store.
        /// </summary>
        public string ShopTexturePath { get; set; }

        /// <summary>
        /// Gets or sets the tint color this paint applies to the super.
        /// </summary>
        public Color SuperTintColor { get; set; }

        /// <summary>
        /// Gets or sets the texture path for the super to use for this color.
        /// Using a tint of White with a custom texture path allows
        /// us to make patterns such as polkadots and more.
        /// </summary>
        public string SuperTexturePath { get; set; }

        /// <summary>
        /// Gets or sets the ID of the MetaSuper this paint will work on.
        /// </summary>
        public int MetaSuperId { get; set; }

        public SuperPaint ToSuperPaint()
        {
            return null;
        }
    }
}
