using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using BusyBeekeeper.Core;

namespace BusyBeekeeper
{
    internal sealed class BeeWorldManagerComponent : GameComponent
    {
        public BeeWorldManagerComponent(Game game)
            : base(game)
        {
        }

        private readonly BeeWorldManager mBeeWorldManager = new BeeWorldManager();

        public BeeWorldManager BeeWorldManager
        {
            get { return this.mBeeWorldManager; }
        }

        public override void Initialize()
        {
            base.Initialize();

            var lContentManager = new ContentManager(this.Game.Services);
            lContentManager.RootDirectory = "Content";

            var lPlayerRepository = new DataRepositories.PlayerAvatarRepository(lContentManager);
            var lBeeYardRepository = new DataRepositories.BeeYardRepository(lContentManager);

            this.BeeWorldManager.PlayerManager.CreateNew(
                0, "Caleb", lPlayerRepository.CreateObject(0), this.mBeeWorldManager,
                lBeeYardRepository);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);            
            this.BeeWorldManager.Update(gameTime.TotalGameTime);
        }
    }
}
