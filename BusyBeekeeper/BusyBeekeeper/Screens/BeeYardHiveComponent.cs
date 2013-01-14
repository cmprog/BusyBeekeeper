using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.DataRepositories;
using BusyBeekeeper.Data;
using BusyBeekeeper.Data.Graphics.BeeYard;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    /// <summary>
    /// This custom component represents a single hive and all its associated information.
    /// </summary>
    internal sealed class BeeYardHiveComponent : ScreenComponent
    {
        private readonly Texture2D mBlankTexture;
        private readonly BeeHive mBeeHive;
        private readonly BeeYardHiveInfo mHiveInfo;
        private readonly SuperRepository mSuperRepository;

        private Vector2 mHivePosition;
        private Vector2 mHiveSize;

        public BeeYardHiveComponent(Texture2D blankTexture, BeeHive beeHive, BeeYardHiveInfo hiveInfo, SuperRepository superRepository)
        {
            this.mBlankTexture = blankTexture;
            this.mBeeHive = beeHive;
            this.mHiveInfo = hiveInfo;
            this.mSuperRepository = superRepository;
        }

        public BeeHive BeeHive
        {
            get { return this.mBeeHive; }
        }

        public event Action<BeeYardHiveComponent> TravelToHive;

        public override bool HandleInput(InputState inputState)
        {
            if (inputState.MouseLeftClickUp())
            {
                var lCurrentMouseState = inputState.CurrentMouseState;
                if (VectorUtilities.HitTest(this.mHivePosition, this.mHiveSize, lCurrentMouseState.X, lCurrentMouseState.Y))
                {
                    this.TravelToHive(this);
                    return true;
                }
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            if (this.mBeeHive.Supers.Count == 0) return;

            var lBasePosition = this.mHiveInfo.Position;
            var lTotalDepth = this.mBeeHive.Supers
                .Select(x => this.mSuperRepository.GetMetaObject(x.MetaId))
                .Sum(x => x.Depth);

            const float lcDepthToHeightFactor = 20f;
            const float lWidth = 75f;

            float lHeight = lcDepthToHeightFactor * lTotalDepth;
            this.mHivePosition = new Vector2(lBasePosition.X, lBasePosition.Y - lHeight);
            this.mHiveSize = new Vector2(lWidth, lHeight);

            spriteBatch.Draw(this.mBlankTexture, this.mHivePosition, null, Color.White, 0, Vector2.Zero, this.mHiveSize, SpriteEffects.None, 0);
        }
    }
}
