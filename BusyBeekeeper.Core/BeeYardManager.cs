using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Data;

namespace BusyBeekeeper.Core
{
    public sealed class BeeYardManager : IUpdatable
    {
        private readonly BeeYard mBeeYard;

        private readonly List<IUpdatable> mUpdatables = new List<IUpdatable>();

        public BeeYardManager(BeeYard beeYard)
        {
            if (beeYard == null) throw new ArgumentNullException("beeYard");
            this.mBeeYard = beeYard;
        }

        public void UpdateTick(BeeWorldManager worldManager)
        {
            foreach (var lUpdatable in this.mUpdatables)
            {
                lUpdatable.UpdateTick(worldManager);
            }
        }
    }
}
