using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TccLib.Xna.Framework.Renderers
{
    /// <summary>
    /// This renderer overrides the basic cababilities of the BasicRenderer
    /// to include all the possible modifications one can perform on a
    /// single texture.
    /// </summary>
    public class AdvancedRenderer : BasicRenderer
    {
        /// <summary>
        /// Initializes a new instance of the AdvancedRenderer class.
        /// </summary>
        /// <param name="textureProperty">A property containing the texture.</param>
        /// <param name="positionProperty">A property containing the position.</param>
        /// <param name="colorProperty">A property containing the color to tint with.</param>
        /// <param name="rotationProperty">A property containing the angular rotation.</param>
        public AdvancedRenderer(
            ISharedProperty<Texture2D> textureProperty,
            ISharedProperty<Vector2> positionProperty,
            ISharedProperty<Color> colorProperty,
            ISharedProperty<float> rotationProperty)
            : this(
                textureProperty,
                positionProperty,
                SharedProperty.Create((Rectangle?)null),
                colorProperty,
                rotationProperty,
                SharedProperty.Create(Vector2.Zero),
                SharedProperty.Create(Vector2.Zero),
                SharedProperty.Create(SpriteEffects.None),
                SharedProperty.Create(1f))
        {
        }

        /// <summary>
        /// Initializes a new instance of the AdvancedRenderer class.
        /// </summary>
        /// <param name="textureProperty">A property containing the texture.</param>
        /// <param name="positionProperty">A property containing the position.</param>
        /// <param name="colorProperty">A property containing the color to tint with.</param>
        /// <param name="scaleProperty">A property containing the scalar vector.</param>
        public AdvancedRenderer(
            ISharedProperty<Texture2D> textureProperty,
            ISharedProperty<Vector2> positionProperty,
            ISharedProperty<Color> colorProperty,
            ISharedProperty<Vector2> scaleProperty)
            : this(
                textureProperty,
                positionProperty,
                SharedProperty.Create((Rectangle?)null),
                colorProperty,
                SharedProperty.Create(0f),
                SharedProperty.Create(Vector2.Zero),
                scaleProperty,
                SharedProperty.Create(SpriteEffects.None),
                SharedProperty.Create(1f))
        {
        }

        /// <summary>
        /// Initializes a new instance of the AdvancedRenderer class.
        /// </summary>
        /// <param name="textureProperty">A property containing the texture.</param>
        /// <param name="positionProperty">A property containing the position.</param>
        /// <param name="colorProperty">A property containing the color to tint with.</param>
        /// <param name="rotationProperty">A property containing the angular rotation.</param>
        /// <param name="scaleProperty">A property containing the scalar vector.</param>
        public AdvancedRenderer(
            ISharedProperty<Texture2D> textureProperty,
            ISharedProperty<Vector2> positionProperty,
            ISharedProperty<Color> colorProperty,
            ISharedProperty<float> rotationProperty,
            ISharedProperty<Vector2> scaleProperty)
            : this(
                textureProperty,
                positionProperty,
                SharedProperty.Create((Rectangle?)null),
                colorProperty,
                rotationProperty,
                SharedProperty.Create(Vector2.Zero),
                scaleProperty,
                SharedProperty.Create(SpriteEffects.None),
                SharedProperty.Create(1f))
        {
        }

        /// <summary>
        /// Initializes a new instance of the AdvancedRenderer class.
        /// </summary>
        /// <param name="textureProperty">A property containing the texture.</param>
        /// <param name="positionProperty">A property containing the position.</param>
        /// <param name="sourceRectangleProperty">A property containing the source rectangle.</param>
        /// <param name="colorProperty">A property containing the color to tint with.</param>
        /// <param name="rotationProperty">A property containing the angular rotation.</param>
        /// <param name="originProperty">A property containing the origin vector.</param>
        /// <param name="scaleProperty">A property containing the scalar vector.</param>
        /// <param name="effectsProperty">A property containing the sprite effects.</param>
        /// <param name="layerDepthProperty">A property containing the layer depth.</param>
        public AdvancedRenderer(
            ISharedProperty<Texture2D> textureProperty,
            ISharedProperty<Vector2> positionProperty,
            ISharedProperty<Rectangle?> sourceRectangleProperty,
            ISharedProperty<Color> colorProperty,
            ISharedProperty<float> rotationProperty,
            ISharedProperty<Vector2> originProperty,
            ISharedProperty<Vector2> scaleProperty,
            ISharedProperty<SpriteEffects> effectsProperty,
            ISharedProperty<float> layerDepthProperty)
            : base(textureProperty, positionProperty, colorProperty)
        {
            System.Diagnostics.Debug.Assert(sourceRectangleProperty != null, "Cannot work with a null source rectangle property.");
            System.Diagnostics.Debug.Assert(rotationProperty != null, "Cannot work with a null rotation property.");
            System.Diagnostics.Debug.Assert(originProperty != null, "Cannot work with a null origin property.");
            System.Diagnostics.Debug.Assert(scaleProperty != null, "Cannot work with a null scale property.");
            System.Diagnostics.Debug.Assert(effectsProperty != null, "Cannot work with a null effects property.");
            System.Diagnostics.Debug.Assert(layerDepthProperty != null, "Cannot work with a null layer depth property.");

            this.SourceRectangleProperty = sourceRectangleProperty;
            this.RotationProperty = rotationProperty;
            this.OriginProperty = originProperty;
            this.ScaleProperty = scaleProperty;
            this.EffectsProperty = effectsProperty;
            this.LayerDepthProperty = layerDepthProperty;
        }

        /// <summary>
        /// Gets the property containing the boundary in the texture to draw.
        /// </summary>
        protected ISharedProperty<Rectangle?> SourceRectangleProperty { get; private set; }

        /// <summary>
        /// Gets the propety containing the angle (in radians) to rotate the
        /// sprite abount its center.
        /// </summary>
        protected ISharedProperty<float> RotationProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the sprite origin.
        /// </summary>
        protected ISharedProperty<Vector2> OriginProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the scale factor.
        /// </summary>
        protected ISharedProperty<Vector2> ScaleProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the effects to apply.
        /// </summary>
        protected ISharedProperty<SpriteEffects> EffectsProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the layer depths with 0 representing the
        /// front layer and 1 representing the back layer.
        /// </summary>
        protected ISharedProperty<float> LayerDepthProperty { get; private set; }

        /// <summary>
        /// Renders the texture by drawing it with the various property values.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw draw sprites.</param>
        /// <param name="gameTime">The current game time.</param>
        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(
                this.TextureProperty.Value,
                this.PositionProperty.Value,
                this.SourceRectangleProperty.Value,
                this.ColorProperty.Value,
                this.RotationProperty.Value,
                this.OriginProperty.Value,
                this.ScaleProperty.Value,
                this.EffectsProperty.Value,
                this.LayerDepthProperty.Value);
        }
    }
}
