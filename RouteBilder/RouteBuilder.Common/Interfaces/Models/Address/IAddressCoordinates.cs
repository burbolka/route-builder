// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAddressCoordinates.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the IAddressCoordinates type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Common.Interfaces.Models.Address
{
    /// <summary>
    /// The AddressCoordinates interface.
    /// </summary>
    public interface IAddressCoordinates
    {
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        double Latitude { get; set; }
    }
}
