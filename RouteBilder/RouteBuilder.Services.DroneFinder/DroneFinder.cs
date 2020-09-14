// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DroneFinder.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the DroneFinder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Services.DroneFinder
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
    using RouteBuilder.Services.DroneFinder.Models;

    /// <summary>
    /// The drone finder.
    /// </summary>
    public class DroneFinder : IDroneFinder
    {
        /// <summary>
        /// The _settings.
        /// </summary>
        private IOptions<List<DroneLocation>> settings;

        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger<DroneFinder> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DroneFinder"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public DroneFinder(IOptions<List<DroneLocation>> settings, ILogger<DroneFinder> logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// The get drone fleets.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{IAddressItem}"/>.
        /// </returns>
        public IEnumerable<IAddressItem> GetDroneFleets()
        {
            try
            {
                if (this.settings.Value.AnySafe())
                {
                    return this.settings.Value.Select(
                        x => new AddressItem { AddressLine = x.AddressLine, Coordinates = x.Coordinates });
                }
            }
            catch (Exception e)
            {
                this.logger.Log(LogLevel.Error, "something went wrong when try get available drones", e);
            }

            return Enumerable.Empty<IAddressItem>();
        }

        /// <summary>
        /// The get available drones.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{IAddressItem}"/>.
        /// </returns>
        public IEnumerable<IAddressItem> GetAvailableDrones()
        {
            try
            {
                var availableDrones = this.settings.Value;
                if (availableDrones.AnySafe())
                {
                    return availableDrones
                        .Where(x => x.HasDrones)
                        .Select(x => new AddressItem { AddressLine = x.AddressLine, Coordinates = x.Coordinates });
                }
            }
            catch (Exception e)
            {
                this.logger.Log(LogLevel.Error, "something went wrong when try get available drones", e);
            }

            return Enumerable.Empty<IAddressItem>();
        }
    }
}
