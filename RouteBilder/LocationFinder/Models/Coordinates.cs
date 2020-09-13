// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Coordinates.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the Coordinates type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Services.LocationFinder.Models
{
    using RouteBuilder.Common.Interfaces.Models.Address;

    /// <summary>
    /// The coordinates.
    /// </summary>
    public class Coordinates : IAddressCoordinates
    {
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Latitude { get; set; }
    }
}
