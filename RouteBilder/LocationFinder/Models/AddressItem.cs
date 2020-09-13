// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddressItem.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the AddressItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Services.LocationFinder.Models
{
    using RouteBuilder.Common.Interfaces.Models.Address;

    /// <summary>
    /// The address item.
    /// </summary>
    public class AddressItem : IAddressItem
    {
        /// <summary>
        /// Gets or sets the address line.
        /// </summary>
        public string AddressLine { get; set; }

        /// <summary>
        /// Gets or sets the coordinates.
        /// </summary>
        public IAddressCoordinates Coordinates { get; set; }
    }
}
