// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILocationFinder.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   The LocationFinder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Common.Interfaces.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RouteBuilder.Common.Interfaces.Models.Address;

    /// <summary>
    /// The LocationFinder interface.
    /// </summary>
    public interface ILocationFinder
    {
        /// <summary>
        /// The get address coordinates.
        /// </summary>
        /// <param name="addressLine">
        /// The address line.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        IAddressItem GetAddressCoordinates(string addressLine);

        /// <summary>
        /// The get address coordinates.
        /// </summary>
        /// <param name="addressLine">
        /// The address line.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        IEnumerable<IAddressItem> GetAddressCoordinates(IEnumerable<string> addressLine);

        /// <summary>
        /// The get all client location.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{IAddressItem}"/>.
        /// </returns>
        IEnumerable<IAddressItem> GetAllClientLocation();
    }
}
