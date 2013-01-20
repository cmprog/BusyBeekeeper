using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens.CommonComponents
{
    internal sealed class InventoryItemSelectorComponent : InventoryItemListComponent
    {
        #region Instance Fields --------------------------------------------------------

        private Action mClosedCallback;
        private Action<InventoryItem> mSelectedCallback;

        private Texture2D mBlankTexture;
        private Button mButtonSelect = new Button();

        #endregion

        #region Constructors -----------------------------------------------------------
        
        public InventoryItemSelectorComponent(Vector2 screenSize)
            : base(screenSize)
        {
        }

        #endregion

        #region Instance Properties ----------------------------------------------------

        public string SelectActionText
        {
            get { return this.mButtonSelect.Text; }
            set { this.mButtonSelect.Text = value; }
        }

        #endregion

        #region Instance Methods -------------------------------------------------------

        /// <summary>
        /// Shows the inventory selector with the given registered callback
        /// which will be called when an item is selected.
        /// </summary>
        /// <param name="inventoryItemSelected">The callback function.</param>
        /// <param name="closedCallback"></param>
        public void Show(Action<InventoryItem> inventoryItemSelected, Action closedCallback)
        {
            this.mClosedCallback = closedCallback;
            this.mSelectedCallback = inventoryItemSelected;
            this.ClearSelection();
            this.Visible = true;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            this.mBlankTexture= contentManager.Load<Texture2D>("Sprites/Blank");

            this.mButtonSelect.Font = contentManager.Load<SpriteFont>("Fonts/DefaultNormal");
            this.mButtonSelect.TextColor = Color.White;
            this.mButtonSelect.BackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.DarkGreen);
            this.mButtonSelect.Size = this.CloseButtonSize;
            this.mButtonSelect.Position = new Vector2(this.CloseButtonPosition.X, this.CloseButtonPosition.Y - this.mButtonSelect.Size.Y - 10f);
            this.mButtonSelect.Click += x => this.SelectCurrentItem();
        }

        private void SelectCurrentItem()
        {
            var lSelectedIndex = this.SelectedIndex;
            if (lSelectedIndex >= 0)
            {
                var lItem = this.Items[lSelectedIndex];
                this.mSelectedCallback(lItem);
                this.Close();
            }
        }

        public override bool HandleInput(InputState inputState)
        {
            if (base.HandleInput(inputState)) return true;
            if (!this.Visible) return false;
            if (this.mButtonSelect.HandleInput(inputState)) return true;
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            if (!this.Visible) return;

            this.mButtonSelect.Draw(spriteBatch, gameTime);
        }

        protected override void Close()
        {
            base.Close();
            if (this.mClosedCallback != null)
            {
                this.mClosedCallback();
            }
        }

        #endregion
    }
}
