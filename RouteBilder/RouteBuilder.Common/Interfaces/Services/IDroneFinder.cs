// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDroneFinder.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   The DroneFinder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Common.Interfaces.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RouteBuilder.Common.Interfaces.Models.Address;

    /// <summary>
    /// The DroneFinder interface.
    /// </summary>
    public interface IDroneFinder
    {
        /// <summary>
        /// The get drone fleets.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{IAddressItem}"/>.
        /// </returns>
        IEnumerable<IAddressItem> GetDroneFleets();

        /// <summary>
        /// The get available drones.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{IAddressItem}"/>.
        /// </returns>
        IEnumerable<IAddressItem> GetAvailableDrones();
    }
}
