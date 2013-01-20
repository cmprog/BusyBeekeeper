using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BusyBeekeeper.Data.Graphics.BeeWorld;

namespace BusyBeekeeper.Screens
{
    internal sealed class BeeWorldScreen : GameScreenBase
    {
        #region Instance Fields --------------------------------------------------------

        private Texture2D mBlankTexture;

        private BeeWorldInfo mBeeWorldInfo;
        private BeeWorldLocationComponent[] mLocationComponents;
        private BeeWorldHudComponent mHudComponent;

        private const int sSpecialHouseCount = 3;
        private const int sShopLocationIndexOffset = 0;
        private const int sMarketLocationIndexOffset = 1;
        private const int sHoneyHouseLocationIndexOffset = 2;

        private const int sShopId = -1;
        private const int sMarketId = -2;
        private const int sHoneyHouseId = -3;

        #endregion

        #region Constructors -----------------------------------------------------------

        #endregion

        #region Instance Properties ----------------------------------------------------

        #endregion

        #region Instance Methods -------------------------------------------------------

        public override void LoadContent()
        {
            base.LoadContent();
            this.mBlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");

            this.mBeeWorldInfo = this.ContentManager.Load<BeeWorldInfo>("GraphicsData/BeeWorld/Info");
            var lBeeYards = this.ScreenManager.Player.BeeYards;

            this.mLocationComponents = new BeeWorldLocationComponent[this.mBeeWorldInfo.WorldYardInfos.Length + sSpecialHouseCount];
            for (int lIndex = 0; lIndex < this.mBeeWorldInfo.WorldYardInfos.Length; lIndex++)
            {
                var lYardInfo = this.mBeeWorldInfo.WorldYardInfos[lIndex];
                var lBeeYard = lBeeYards[lYardInfo.Id];

                var lLocationComponent = new BeeWorldLocationComponent();
                lLocationComponent.IsEnabled = lBeeYard.IsUnlocked;
                lLocationComponent.Tag = lBeeYard.Id;
                lLocationComponent.LoadContent(this.ContentManager);
                lLocationComponent.NameText = lBeeYard.Name;
                lLocationComponent.NamePosition = lYardInfo.NamePosition;
                lLocationComponent.NameSize = lYardInfo.NameSize;
                lLocationComponent.Click += this.BeeWorldLocationComponent_Click;

                this.mLocationComponents[lIndex] = lLocationComponent;
            }

            var lShopLocationComponent = new BeeWorldLocationComponent();
            lShopLocationComponent.IsEnabled = true;
            lShopLocationComponent.Tag = sShopId;
            lShopLocationComponent.NameText = "Barry's Bee Emporium";
            lShopLocationComponent.NamePosition = this.mBeeWorldInfo.ShopInfo.NamePosition;
            lShopLocationComponent.NameSize = this.mBeeWorldInfo.ShopInfo.NameSize;
            lShopLocationComponent.LoadContent(this.ContentManager);
            lShopLocationComponent.Click += this.BeeWorldLocationComponent_Click;
            this.mLocationComponents[this.mBeeWorldInfo.WorldYardInfos.Length + sShopLocationIndexOffset] = lShopLocationComponent;

            var lMarketLocationComponent = new BeeWorldLocationComponent();
            lMarketLocationComponent.IsEnabled = false;
            lMarketLocationComponent.Tag = sMarketId;
            lMarketLocationComponent.NameText = "Farmer's Market";
            lMarketLocationComponent.NamePosition = this.mBeeWorldInfo.MarketInfo.NamePosition;
            lMarketLocationComponent.NameSize = this.mBeeWorldInfo.MarketInfo.NameSize;
            lMarketLocationComponent.LoadContent(this.ContentManager);
            lMarketLocationComponent.Click += this.BeeWorldLocationComponent_Click;
            this.mLocationComponents[this.mBeeWorldInfo.WorldYardInfos.Length + sMarketLocationIndexOffset] = lMarketLocationComponent;

            var lHoneyHouseLocationComponent = new BeeWorldLocationComponent();
            lHoneyHouseLocationComponent.IsEnabled = true;
            lHoneyHouseLocationComponent.Tag = sHoneyHouseId;
            lHoneyHouseLocationComponent.NameText = "Honey House";
            lHoneyHouseLocationComponent.NamePosition = this.mBeeWorldInfo.HoneyHouseInfo.NamePosition;
            lHoneyHouseLocationComponent.NameSize = this.mBeeWorldInfo.HoneyHouseInfo.NameSize;
            lHoneyHouseLocationComponent.LoadContent(this.ContentManager);
            lHoneyHouseLocationComponent.Click += this.BeeWorldLocationComponent_Click;
            this.mLocationComponents[this.mBeeWorldInfo.WorldYardInfos.Length + sHoneyHouseLocationIndexOffset] = lHoneyHouseLocationComponent;

            this.mHudComponent = new BeeWorldHudComponent(this.ScreenManager.BeeWorldManager, this.ScreenSize);
            this.mHudComponent.LoadContent(this.ContentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var lLocationComponent in this.mLocationComponents) lLocationComponent.Update(gameTime);
            this.mHudComponent.Update(gameTime);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            foreach (var lLocationComponent in this.mLocationComponents)
            {
                if (lLocationComponent.HandleInput(inputState)) return;
            }

            if (this.mHudComponent.HandleInput(inputState)) return;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            
            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);
            foreach (var lLocationComponent in this.mLocationComponents) lLocationComponent.Draw(spriteBatch, gameTime);
            this.mHudComponent.Draw(spriteBatch, gameTime);
        }

        private void BeeWorldLocationComponent_Click(BeeWorldLocationComponent locationComponent)
        {
            var lLocationId = (int) locationComponent.Tag;
            var lPlayerManager = this.ScreenManager.BeeWorldManager.PlayerManager;

            switch (lLocationId)
            {
                case sShopId:
                    {
                        var lShopScreen = new ShopScreen();
                        var lTravelingScreen = new TravelingScreen(lShopScreen);
                        this.ScreenManager.TransitionTo(lTravelingScreen);
                        lPlayerManager.TravelToShop(lTravelingScreen.TravelingComplete);
                        break;
                    }

                case sMarketId:
                    System.Diagnostics.Debug.Assert(false, "Market location should be disabled.");
                    break;

                case sHoneyHouseId:
                    {
                        var lHoneyHouseScreen = new HoneyHouseScreen();
                        var lTravelingScreen = new TravelingScreen(lHoneyHouseScreen);
                        this.ScreenManager.TransitionTo(lTravelingScreen);
                        lPlayerManager.TravelToHoneyHouse(lTravelingScreen.TravelingComplete);
                        break;
                    }

                default:
                    {
                        var lPlayer = lPlayerManager.Player;
                        var lBeeYard = lPlayer.BeeYards[lLocationId];
                        System.Diagnostics.Debug.Assert(lBeeYard.IsUnlocked);

                        var lBeeYardScreen = new BeeYardScreen();

                        if (lBeeYard == lPlayerManager.Player.CurrentBeeYard)
                        {
                            lPlayerManager.TravelToBeeYard();
                            this.ScreenManager.TransitionTo(lBeeYardScreen);
                        }
                        else
                        {
                            var lTravelingScreen = new TravelingScreen(lBeeYardScreen);
                            this.ScreenManager.TransitionTo(lTravelingScreen);

                            lPlayerManager.TravelTo(lBeeYard, lTravelingScreen.TravelingComplete);
                        }

                        break;
                    }
            }
            

        }

        #endregion
    }
}
