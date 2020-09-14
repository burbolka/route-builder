// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocationFinder.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the LocationFinder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Services.LocationFinder
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
    using RouteBuilder.Services.LocationFinder.Models;

    /// <summary>
    /// The location finder.
    /// </summary>
    public class LocationFinder : ILocationFinder
    {
        /// <summary>
        /// The _settings.
        /// </summary>
        private IOptions<List<LocationSetting>> settings;

        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger<LocationFinder> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationFinder"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public LocationFinder(IOptions<List<LocationSetting>> settings, ILogger<LocationFinder> logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// The get address coordinates.
        /// </summary>
        /// <param name="addressLine">
        /// The address line.
        /// </param>
        /// <returns>
        /// The <see cref="IAddressItem"/>
        /// address item
        /// </returns>
        public IAddressItem GetAddressCoordinates(string addressLine)
        {
            try
            {
                if (!this.settings.Value.AnySafe())
                {
                    return null;
                }

                var address = this.settings.Value.FirstOrDefault(
                    x => x.AreEqual(addressLine));

                if (address != null)
                {
                    return new AddressItem { AddressLine = address.AddressLine, Coordinates = address.Coordinates };
                }
            }
            catch (Exception e)
            {
                this.logger.Log(LogLevel.Error, "something went wrong when try get stores", e);
            }

            return null;
        }

        /// <summary>
        /// The get address coordinates.
        /// </summary>
        /// <param name="addressLine">
        /// The address line.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{IAddressItem}"/>.
        /// </returns>
        public IEnumerable<IAddressItem> GetAddressCoordinates(IEnumerable<string> addressLine)
        {
            try
            {
                var availableStores = this.settings.Value;
                if (availableStores.AnySafe())
                {
                    return availableStores
                        .Where(x => x.AreEqual(addressLine))
                        .Select(x => new AddressItem { AddressLine = x.AddressLine, Coordinates = x.Coordinates });
                }
            }
            catch (Exception e)
            {
                this.logger.Log(LogLevel.Error, "something went wrong when try get stores", e);
            }

            return Enumerable.Empty<IAddressItem>();
        }

        /// <summary>
        /// The get all client location.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{IAddressItem}"/>.
        /// </returns>
        public IEnumerable<IAddressItem> GetAllClientLocation()
        {
            try
            {
                var availableStores = this.settings.Value;
                if (availableStores.AnySafe())
                {
                    return availableStores.Select(
                        x => new AddressItem { AddressLine = x.AddressLine, Coordinates = x.Coordinates });
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
