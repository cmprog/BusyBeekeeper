using System;
using System.Collections.Generic;
using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using Microsoft.Xna.Framework.Content;

namespace BusyBeekeeper.DataRepositories
{
    internal sealed class QueenBeeRepository : IMetaObjectRepository<MetaQueenBee, QueenBee>
    {
        private MetaQueenBee[] mMetaQueenBees;

        public QueenBeeRepository(ContentManager contentManager)
        {
            this.mMetaQueenBees = contentManager.Load<MetaQueenBee[]>("Data/QueenBees");
        }

        public MetaQueenBee GetMetaObject(int metaId)
        {
            return this.mMetaQueenBees[metaId];
        }

        public QueenBee CreateObject(int metaId)
        {
            return this.CreateObject(this.GetMetaObject(metaId));
        }

        public QueenBee CreateObject(MetaQueenBee metaObject)
        {
            var lQueenBee = new QueenBee();

            lQueenBee.MetaId = metaObject.Id;
            
            lQueenBee.Name = metaObject.Name;
            lQueenBee.Description = metaObject.Description;

            lQueenBee.BeePopulationGrowthFactor = metaObject.BeePopulationGrowthFactor;
            lQueenBee.HoneyCollectionFactor = metaObject.HoneyCollectionFactor;
            lQueenBee.ColonyStrengthFactor = metaObject.ColonyStrengthFactor;
            lQueenBee.NaturalBeeAgressionFactor = metaObject.NaturalBeeAgressionFactor;
            lQueenBee.SwarmLikelinessFactor = metaObject.SwarmLikelinessFactor;

            return lQueenBee;
        }
    }
}
