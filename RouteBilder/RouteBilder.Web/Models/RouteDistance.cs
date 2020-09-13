// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteDistance.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the RouteDistance type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Web.Models
{
    using RouteBuilder.Common.Interfaces.Models.Address;

    /// <summary>
    /// The route distance.
    /// </summary>
    public class RouteDistance
    {
        /// <summary>
        /// Gets or sets the location from.
        /// </summary>
        public string LocationFrom { get; set; }

        /// <summary>
        /// Gets or sets the location to.
        /// </summary>
        public string LocationTo { get; set; }

        /// <summary>
        /// Gets or sets the distance.
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Gets or sets the location from coordinates.
        /// </summary>
        public IAddressItem LocationFromAddress { get; set; }

        /// <summary>
        /// Gets or sets the location to coordinates.
        /// </summary>
        public IAddressItem LocationToAddress { get; set; }
    }
}
