using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal sealed class ShopScreenHudComponent : HudComponent
    {
        #region Instance Fields --------------------------------------------------------

        private readonly Player mPlayer;

        private Vector2 mAvailableCoinsTextPosition;

        #endregion

        #region Constructors -----------------------------------------------------------

        public ShopScreenHudComponent(BeeWorldManager worldManager, Vector2 screenSize)
            : base(worldManager, screenSize)
        {
            this.mPlayer = worldManager.PlayerManager.Player;
        }

        #endregion

        #region Instance Properties ----------------------------------------------------

        #endregion

        #region Instance Methods -------------------------------------------------------

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            
            var lSampleTextSize = this.Font.MeasureString("X");
            this.mAvailableCoinsTextPosition = new Vector2(
                this.Position.X + sItemMargin,
                this.Position.Y + this.Size.Y - sItemMargin - lSampleTextSize.Y);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            var lAvailableCoins = this.mPlayer.AvailableCoins;
            var lAvailableCoinsText = string.Concat(lAvailableCoins, " coins");

            spriteBatch.DrawString(this.Font, lAvailableCoinsText, this.mAvailableCoinsTextPosition, Color.Black);
        }

        #endregion
        
    }
}
