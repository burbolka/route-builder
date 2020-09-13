// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAddressText.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the IAddressText type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Common.Interfaces.Models.Address
{
    /// <summary>
    /// The AddressText interface.
    /// </summary>
    public interface IAddressText
    {
        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// Gets or sets the line 1.
        /// </summary>
        string Line1 { get; set; }

        /// <summary>
        /// Gets or sets the line 2.
        /// </summary>
        string Line2 { get; set; }
    }
}
