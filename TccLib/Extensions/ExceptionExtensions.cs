using System;

namespace TccLib.Extensions
{
    /// <summary>
    /// This collection of extension methods are useful for throwing exceptions.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// If the given value is Null, throws an ArgumentNullException with the given parameter name.
        /// </summary>
        /// <typeparam name="T">The type of the value we're checking against.</typeparam>
        /// <param name="value">The value we're checking for nullness.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The value of the parameter.</returns>
        public static T ThrowIfNull<T>(this T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }
    }
}
