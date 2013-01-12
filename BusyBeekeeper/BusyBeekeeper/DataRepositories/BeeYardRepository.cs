using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using Microsoft.Xna.Framework.Content;

namespace BusyBeekeeper.DataRepositories
{
    internal sealed class BeeYardRepository : IObjectRepository<BeeYard>
    {
        private readonly BeeYard[] mBeeYards;

        public BeeYardRepository(ContentManager contentManager)
        {
            System.Diagnostics.Debug.Assert(contentManager != null);
            this.mBeeYards = contentManager.Load<BeeYard[]>("Data/BeeYards");
        }

        public int Count
        {
            get { return this.mBeeYards.Length; }
        }

        public BeeYard CreateObject(int id)
        {
            return this.mBeeYards[id];
        }
    }
}
