using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using Microsoft.Xna.Framework.Content;

namespace BusyBeekeeper.DataRepositories
{
    internal sealed class LawnMowerRepository : IObjectRepository<LawnMower>
    {
        private readonly LawnMower[] mLawnMowers;

        public LawnMowerRepository(ContentManager contentManager)
        {
            System.Diagnostics.Debug.Assert(contentManager != null);
            this.mLawnMowers = contentManager.Load<LawnMower[]>("Data/LawnMowers");
        }

        public int Count
        {
            get { return this.mLawnMowers.Length; }
        }

        public LawnMower CreateObject(int id)
        {
            return this.mLawnMowers[id];
        }
    }
}
