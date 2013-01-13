using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using Microsoft.Xna.Framework.Content;

namespace BusyBeekeeper.DataRepositories
{
    internal sealed class SuperRepository : IMetaObjectRepository<MetaSuper, Super>
    {
        private const int sSuperCount = 3;
        private readonly MetaSuper[] mMetaSupers = new MetaSuper[sSuperCount];

        public SuperRepository(ContentManager contentManager)
        {
            this.mMetaSupers[0] = new MetaSuper { Id = 0, Name = "Shallow", Description = "A small cheap super to collect honey.", Depth = 1, PurchasePrice = 100 };
            this.mMetaSupers[1] = new MetaSuper { Id = 1, Name = "Medium", Description = "A medium sized super to collect a good amount of honey.", Depth = 2, PurchasePrice = 1000 };
            this.mMetaSupers[2] = new MetaSuper { Id = 2, Name = "Deep", Description = "A large super to collect lotsa honey.", Depth = 3, PurchasePrice = 5000 };
        }

        public MetaSuper GetMetaObject(int metaId)
        {
            return this.mMetaSupers[metaId];
        }

        public Super CreateObject(int metaId)
        {
            return new Super { MetaId = metaId, SuperPaint = null };
        }

        public Super CreateObject(MetaSuper metaObject)
        {
            return this.CreateObject(metaObject.Id);
        }
    }
}
