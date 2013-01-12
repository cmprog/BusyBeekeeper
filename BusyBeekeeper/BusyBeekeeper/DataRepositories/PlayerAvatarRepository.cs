using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using Microsoft.Xna.Framework.Content;

namespace BusyBeekeeper.DataRepositories
{
    internal sealed class PlayerAvatarRepository : IMetaObjectRepository<MetaPlayerAvatar, PlayerAvatar>
    {
        private const int sAvatarCount = 3;
        private readonly MetaPlayerAvatar[] mMetaAvatars = new MetaPlayerAvatar[sAvatarCount];

        public PlayerAvatarRepository(ContentManager contentManager)
        {
            mMetaAvatars[0] = new MetaPlayerAvatar { Id = 0, Name = "Boy" };
            mMetaAvatars[1] = new MetaPlayerAvatar { Id = 1, Name = "Girl" };
            mMetaAvatars[2] = new MetaPlayerAvatar { Id = 2, Name = "Bear" };
        }

        public MetaPlayerAvatar GetMetaObject(int metaId)
        {
            return this.mMetaAvatars[metaId];
        }

        public PlayerAvatar CreateObject(int metaId)
        {
            return new PlayerAvatar { MetaId = metaId };
        }

        public PlayerAvatar CreateObject(MetaPlayerAvatar metaObject)
        {
            return new PlayerAvatar { MetaId = metaObject.Id };
        }
    }
}
