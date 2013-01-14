using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using Microsoft.Xna.Framework.Content;

namespace BusyBeekeeper.DataRepositories
{
    internal sealed class SmokerRepository : IObjectRepository<Smoker>
    {
        private readonly Smoker[] mSmokers;

        public SmokerRepository(ContentManager contentManager)
        {
            System.Diagnostics.Debug.Assert(contentManager != null);
            this.mSmokers = contentManager.Load<Smoker[]>("Data/Smokers");
        }

        public int Count
        {
            get { return this.mSmokers.Length; }
        }

        public Smoker CreateObject(int id)
        {
            return this.mSmokers[id];
        }
    }
}
