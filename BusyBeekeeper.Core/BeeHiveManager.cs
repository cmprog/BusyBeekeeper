using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Data;

namespace BusyBeekeeper.Core
{
    internal sealed class BeeHiveManager : IUpdatable
    {
        private readonly BeeHive mBeeHive;

        public BeeHiveManager(BeeHive beeHive)
        {
            if (beeHive == null) throw new ArgumentNullException("beeHive");
            this.mBeeHive = beeHive;
        }

        public void UpdateTick(BeeWorldManager worldManager)
        {
            if (this.mBeeHive.Supers.Count == 0) return;
        }
    }
}
