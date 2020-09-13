// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAddressItem.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   The AddressItem interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Common.Interfaces.Models.Address
{
    /// <summary>
    /// The AddressItem interface.
    /// </summary>
    public interface IAddressItem
    {
        /// <summary>
        /// Gets or sets the address line.
        /// </summary>
        string AddressLine { get; set; }

        /// <summary>
        /// Gets or sets the coordinates.
        /// </summary>
        IAddressCoordinates Coordinates { get; set; }
    }
}
