using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Core;
using Microsoft.Xna.Framework;

namespace BusyBeekeeper.Screens
{
    internal sealed class BeeHiveHudComponent : HudComponent
    {
        public BeeHiveHudComponent(BeeWorldManager beeWorldManager, Vector2 screenSize)
            : base(beeWorldManager, screenSize)
        {
        }
    }
}
