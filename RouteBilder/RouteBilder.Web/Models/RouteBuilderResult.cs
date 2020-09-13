// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteBuilderResult.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the RouteBuilderResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Web.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The route builder result.
    /// </summary>
    public class RouteBuilderResult
    {
        /// <summary>
        /// Gets or sets the route variants.
        /// </summary>
        public IEnumerable<RouteSettings> RouteVariants { get; set; }

        /// <summary>
        /// Gets or sets the selected route.
        /// </summary>
        public RouteSettings SelectedRoute { get; set; }
    }
}
