// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the EnumerableExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Common.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The enumerable extensions.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// The any safe.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <typeparam name="T">
        /// any type
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool AnySafe<T>(this IEnumerable<T> target)
        {
            return target != null && target.Any();
        }

        /// <summary>
        /// The any safe.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <typeparam name="T">
        /// any type
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool AnySafe<T>(this IEnumerable<T> target, Func<T, bool> predicate)
        {
            return target != null && target.Any(predicate);
        }
    }
}
