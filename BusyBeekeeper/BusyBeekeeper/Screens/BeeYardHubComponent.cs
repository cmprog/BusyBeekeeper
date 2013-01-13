using System;
using BusyBeekeeper.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace BusyBeekeeper.Screens
{
    internal sealed class BeeYardHubComponent : HudComponent
    {
        public BeeYardHubComponent(BeeWorldManager worldManager, Vector2 screenSize)
            : base(worldManager, screenSize)
        {
        }
    }
}
