﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Data.Meta
{
    /// <summary>
    /// Defines the persisted resource information about what makes a
    /// lawn mower.
    /// </summary>
    public class MetaLawnMower
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
        /// Gets or sets a factor which influences how much more aggressive
        /// bees are after mowing the lawn.
        /// </summary>
        public float BeeAgressionFactor { get; set; }

        /// <summary>
        /// Gets or sets a factor which influences the speed at which the player
        /// can mow the lawn.
        /// </summary>
        public float MowingSpeedFactor { get; set; }
    }
}