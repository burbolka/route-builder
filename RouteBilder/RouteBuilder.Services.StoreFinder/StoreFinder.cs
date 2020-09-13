// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoreFinder.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the StoreFinder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Services.StoreFinder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using RouteBuilder.Common.ExtensionMethods;
    using RouteBuilder.Common.Interfaces.Models.Address;
    using RouteBuilder.Common.Interfaces.Services;
    using RouteBuilder.Services.StoreFinder.Models;

    /// <summary>
    /// The store finder.
    /// </summary>
    public class StoreFinder : IStoreFinder
    {
        /// <summary>
        /// The _settings.
        /// </summary>
        private IOptions<List<StoreLocation>> settings;

        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger<StoreFinder> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreFinder"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public StoreFinder(IOptions<List<StoreLocation>> settings, ILogger<StoreFinder> logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// The get stores to serve.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{IAddressItem}"/>.
        /// </returns>
        public async Task<IEnumerable<IAddressItem>> GetStoresToServe()
        {
            try
            {
                var availableStores = this.settings.Value;
                if (availableStores.AnySafe())
                {
                    return availableStores
                        .Where(x => x.CanServe)
                        .Select(x => new AddressItem { AddressLine = x.AddressLine, Coordinates = x.Coordinates });
                }
            }
            catch (Exception e)
            {
                this.logger.Log(LogLevel.Error, "something went wrong when try get stores", e);
            }

            return Enumerable.Empty<IAddressItem>();
        }
    }
}
