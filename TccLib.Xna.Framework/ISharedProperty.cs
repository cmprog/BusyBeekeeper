namespace TccLib.Xna.Framework
{
    /// <summary>
    /// A shared property is a wrapper is simply a generic wrapper around a strongly typed value.
    /// </summary>
    /// <remarks>
    /// Shared properties are good for passing sharing fields between components, behaviors, and messages.
    /// </remarks>
    /// <typeparam name="T">The type of the value contained by this property.</typeparam>
    public interface ISharedProperty<T>
    {
        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        T Value { get; set; }
    }
}
