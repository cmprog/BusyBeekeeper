using System;
using System.Linq;
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
        #region Instance Fields --------------------------------------------------------

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

        #endregion

        #region Constructors -----------------------------------------------------------

        public event Action<BeeYardHiveComponent> TravelToHive;

        #endregion

        #region Instance Properties ----------------------------------------------------

        public BeeHive BeeHive
        {
            get { return this.mBeeHive; }
        }

        #endregion

        #region Instance Methods -------------------------------------------------------

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

            const float lcDepthToHeightFactor = 15f;
            const float lcSuperWidth = 75f;

            var lSuperPosition = new Vector2(this.mHiveInfo.Position.X, this.mHiveInfo.Position.Y - 1);
            var lSuperSize = new Vector2(lcSuperWidth, 0);
            for (int lIndex = 0; lIndex < this.mBeeHive.Supers.Count; lIndex++)
            {
                var lSuper = this.mBeeHive.Supers[lIndex];
                lSuperSize.Y = lSuper.Depth * lcDepthToHeightFactor;
                lSuperPosition.Y -= lSuperSize.Y + 1;

                spriteBatch.Draw(this.mBlankTexture, lSuperPosition, null, Color.White, 0, Vector2.Zero, lSuperSize, SpriteEffects.None, 0);
            }
            
            const float lcStandHeight = 8f;
            const float lcStandWidth = lcSuperWidth;
            var lStandSize = new Vector2(lcStandWidth, lcStandHeight);

            spriteBatch.Draw(this.mBlankTexture, this.mHiveInfo.Position, null, Color.SandyBrown, 0, Vector2.Zero, lStandSize, SpriteEffects.None, 0);

            this.mHivePosition = lSuperPosition;
            this.mHiveSize = new Vector2(lcStandWidth, this.mHiveInfo.Position.Y + lcStandHeight - lSuperPosition.Y);
        }

        #endregion
    }
}
