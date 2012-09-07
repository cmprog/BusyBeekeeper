using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Components
{
    /// <summary>
    /// This Component defines the properties associated with a
    /// button for selecting a GameSlot when continuing an existing
    /// game or creating a new game.
    /// </summary>
    public class GameSlotButtonComponent : Component
    {
        /// <summary>
        /// Creates a new instance of the GameSlotButtonComponent class.
        /// </summary>
        public GameSlotButtonComponent()
        {
            this.SizeProperty = SharedProperty.Create(Vector2.Zero);
            this.PositionProperty = SharedProperty.Create(Vector2.Zero);
            this.BeekeeperName = SharedProperty.Create(string.Empty);
            this.TotalTimePlayed = SharedProperty.Create(TimeSpan.Zero);
            this.AwardCount = SharedProperty.Create(0);
            this.NameFont = SharedProperty.Create(default(SpriteFont));
            this.DetailFont = SharedProperty.Create(default(SpriteFont));
            this.AvatarTexture = SharedProperty.Create(default(Texture2D));
            this.AvatarPosition = SharedProperty.Create(Vector2.Zero);
            this.AvatarSize = SharedProperty.Create(Vector2.Zero);
        }

        /// <summary>
        /// Gets the property containing the size of the button.
        /// </summary>
        public SharedProperty<Vector2> SizeProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the position of the button.
        /// </summary>
        public SharedProperty<Vector2> PositionProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the player's Beekeeper name.
        /// </summary>
        public SharedProperty<string> BeekeeperName { get; private set; }

        /// <summary>
        /// Gets the property containing the total time the player
        /// has played under this game slot.
        /// </summary>
        public SharedProperty<TimeSpan> TotalTimePlayed { get; private set; }

        /// <summary>
        /// Gets the number of awards this player has earned under this account.
        /// </summary>
        public SharedProperty<int> AwardCount { get; private set; }

        /// <summary>
        /// Gets the property containing the font used to render the
        /// beekeeper's name.
        /// </summary>
        public SharedProperty<SpriteFont> NameFont { get; private set; }

        /// <summary>
        /// Gets the property containing the font used for drawing the
        /// misc information about the player.
        /// </summary>
        public SharedProperty<SpriteFont> DetailFont { get; private set; }

        /// <summary>
        /// Gets the property containing the Texture of the avatar
        /// related to this component.
        /// </summary>
        public SharedProperty<Texture2D> AvatarTexture { get; private set; }

        /// <summary>
        /// Gets the property containing the position of the players avatar.
        /// </summary>
        public SharedProperty<Vector2> AvatarPosition { get; private set; }

        /// <summary>
        /// Gets the property containing the size of the avatar.
        /// </summary>
        public SharedProperty<Vector2> AvatarSize { get; private set; }

        /// <summary>
        /// Gets or sets a randerer for this component.
        /// </summary>
        public IRenderer Renderer { get; set; }
    }
}
