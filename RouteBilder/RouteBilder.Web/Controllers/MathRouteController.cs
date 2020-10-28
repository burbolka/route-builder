// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathRouteController.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the MathRouteController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using RouteBuilder.Common.ExtensionMethods;
    using RouteBuilder.Common.Helpers;
    using RouteBuilder.Common.Interfaces.Models.Address;
    using RouteBuilder.Common.Interfaces.Services;
    using RouteBuilder.Services.LocationFinder.Models;
    using RouteBuilder.Web.Helpers;
    using RouteBuilder.Web.Models;

    using AddressItem = RouteBuilder.Web.Models.AddressItem;

    /// <summary>
    /// The math route controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MathRouteController : ControllerBase
    {
        /// <summary>
        /// The drone service.
        /// </summary>
        private readonly IDroneFinder droneService;

        /// <summary>
        /// The store finder.
        /// </summary>
        private readonly IStoreFinder storeFinder;

        /// <summary>
        /// The location finder.
        /// </summary>
        private readonly ILocationFinder locationFinder;

        /// <summary>
        /// The _settings.
        /// </summary>
        private IOptions<List<LocationSetting>> clientLocations;

        /// <summary>
        /// Initializes a new instance of the <see cref="MathRouteController"/> class.
        /// </summary>
        /// <param name="droneFinder">
        /// The drone finder.
        /// </param>
        /// <param name="storeFinder">
        /// The store finder.
        /// </param>
        /// <param name="locationFinder">
        /// The location finder.
        /// </param>
        /// <param name="clientLocations">
        /// The client locations.
        /// </param>
        public MathRouteController(
            IDroneFinder droneFinder,
            IStoreFinder storeFinder,
            ILocationFinder locationFinder,
            IOptions<List<LocationSetting>> clientLocations)
        {
            this.droneService = droneFinder;
            this.storeFinder = storeFinder;
            this.locationFinder = locationFinder;
            this.clientLocations = clientLocations;
        }

        /// <summary>
        /// The get for all.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Route("GetForAll")]
        public async Task<IActionResult> GetForAll()
        {
            // get stores
            var stores = this.storeFinder.GetStoresToServe().ToList();
            var fleets = this.droneService.GetDroneFleets().ToList();

            var clientAddresses = await this.locationFinder.GetAllClientLocationAsync();

            var result = new List<RouteSettings>();

            if (!stores.AnySafe() || !fleets.AnySafe() || !clientAddresses.AnySafe())
            {
                return null;
            }

            var calc = new DistanceCalculator();

            var fleetStoreDistanceArr = new List<List<double>>();
            var line = new LinkedList<double>();
            foreach (var fleet in fleets)
            {
                line.Clear();
                foreach (var store in stores)
                {
                    line.AddLast(calc.Calculate(fleet.Coordinates, store.Coordinates));
                }

                fleetStoreDistanceArr.Add(line.ToList());
            }

            var ways = new List<BestWay>();
            var distance = new List<double>();
            var bestWay = new BestWay();
            foreach (var client in clientAddresses)
            {
                distance.Clear();
                foreach (var store in stores)
                {
                    distance.Add(calc.Calculate(client.Coordinates, store.Coordinates));
                }

                bestWay = new BestWay
                {
                    Distance = double.MaxValue,
                    FleetIndex = 0,
                    StoreIndex = 0,
                    Client = client
                };

                var availableDrones = await this.droneService.GetAvailableDronesAsync();

                for (var i = 0; i < fleetStoreDistanceArr.Count; i++)
                {
                    // for loop bellow fleetStoreDistanceArr line length will be the same for all lines,
                    // so we don't need to calculate it for every line
                    for (var j = 0; j < fleetStoreDistanceArr[0].Count; j++)
                    {
                        if (availableDrones.Any(
                            x => x.AddressLine.Equals(
                                fleets[i].AddressLine,
                                StringComparison.InvariantCultureIgnoreCase)))
                        {
                            if (fleetStoreDistanceArr[i][j] + distance[j] < bestWay.Distance)
                            {
                                bestWay.Distance = fleetStoreDistanceArr[i][j] + distance[j];
                                bestWay.FleetIndex = i;
                                bestWay.StoreIndex = j;
                            }
                        }
                    }
                }

                ways.Add(bestWay);
            }

            foreach (var way in ways)
            {
                result.Add(
                    BuildRouteHelper.BuildRouteSettings(
                        calc,
                        new RouteDistance
                        {
                            LocationFrom = fleets[way.FleetIndex].AddressLine,
                            LocationFromAddress = fleets[way.FleetIndex],
                            LocationTo = stores[way.StoreIndex].AddressLine,
                            LocationToAddress = stores[way.StoreIndex]
                        },
                        new RouteDistance
                        {
                            LocationFrom = stores[way.StoreIndex].AddressLine,
                            LocationFromAddress = stores[way.StoreIndex],
                            LocationTo = way.Client.AddressLine,
                            LocationToAddress = way.Client
                        },
                        way.Distance));
            }

            return this.Ok(result);
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Route("Get/{address}")]
        public async Task<IActionResult> Get(string address)
        {
            // get stores
            var stores = this.storeFinder.GetStoresToServe().ToList();
            var fleets = this.droneService.GetAvailableDrones().ToList();
            var clientAddress = await this.locationFinder.GetAddressCoordinatesAsync(address);

            if (!stores.AnySafe() || !fleets.AnySafe() || clientAddress == null)
            {
                return null;
            }

            var calc = new DistanceCalculator();

            var fleetStoreDistanceArr = new List<List<double>>();
            var line = new LinkedList<double>();
            foreach (var fleet in fleets)
            {
                line.Clear();
                foreach (var store in stores)
                {
                    line.AddLast(calc.Calculate(fleet.Coordinates, store.Coordinates));
                }

                fleetStoreDistanceArr.Add(line.ToList());
            }

            var distance = new List<double>();
            foreach (var store in stores)
            {
                distance.Add(calc.Calculate(clientAddress.Coordinates, store.Coordinates));
            }

            var bestWay = new BestWay
            {
                Distance = double.MaxValue,
                FleetIndex = 0,
                StoreIndex = 0,
                Client = clientAddress
            };

            for (var i = 0; i < fleetStoreDistanceArr.Count; i++)
            {
                // for loop bellow fleetStoreDistanceArr line length will be the same for all lines,
                // so we don't need to calculate it for every line
                for (var j = 0; j < fleetStoreDistanceArr[0].Count; j++)
                {
                    if (fleetStoreDistanceArr[i][j] + distance[j] < bestWay.Distance)
                    {
                        bestWay.Distance = fleetStoreDistanceArr[i][j] + distance[j];
                        bestWay.FleetIndex = i;
                        bestWay.StoreIndex = j;
                    }
                }
            }

            return this.Ok(BuildRouteHelper.BuildRouteSettings(
                calc,
                new RouteDistance
                {
                    LocationFrom = fleets[bestWay.FleetIndex].AddressLine,
                    LocationFromAddress = fleets[bestWay.FleetIndex],
                    LocationTo = stores[bestWay.StoreIndex].AddressLine,
                    LocationToAddress = stores[bestWay.StoreIndex]
                },
                new RouteDistance
                {
                    LocationFrom = stores[bestWay.StoreIndex].AddressLine,
                    LocationFromAddress = stores[bestWay.StoreIndex],
                    LocationTo = bestWay.Client.AddressLine,
                    LocationToAddress = bestWay.Client
                },
                bestWay.Distance));
        }
    }
}
