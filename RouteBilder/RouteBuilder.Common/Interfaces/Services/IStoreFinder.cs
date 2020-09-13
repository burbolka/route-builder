// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStoreFinder.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the IStoreFinder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Common.Interfaces.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RouteBuilder.Common.Interfaces.Models.Address;

    /// <summary>
    /// The StoreFinder interface.
    /// </summary>
    public interface IStoreFinder
    {
        /// <summary>
        /// The get stores to serve.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{IAddressItem}"/>.
        /// </returns>
        Task<IEnumerable<IAddressItem>> GetStoresToServe();
    }
}
