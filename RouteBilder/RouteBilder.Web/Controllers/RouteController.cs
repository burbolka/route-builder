// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteController.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   The route controller.
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
    using RouteBuilder.Common.Interfaces.Services;
    using RouteBuilder.Services.LocationFinder.Models;
    using RouteBuilder.Web.Models;
    using Address = Models.AddressItem;

    /// <summary>
    /// The route controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
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
        /// Initializes a new instance of the <see cref="RouteController"/> class.
        /// </summary>
        /// <param name="droneFinder">
        /// The drone finder.
        /// </param>
        /// <param name="storeFinder">
        /// The store Finder.
        /// </param>
        /// <param name="locationFinder">
        /// The location Finder.
        /// </param>
        /// <param name="clientLocations">
        /// The client Locations.
        /// </param>
        public RouteController(
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
            var stores = (await this.storeFinder.GetStoresToServe()).ToList();
            var fleets = (await this.droneService.GetDroneFleets()).ToList();
            var clientAddresses = this.clientLocations.Value;

            if (!stores.AnySafe() || !fleets.AnySafe() || !clientAddresses.AnySafe())
            {
                return this.Ok("cannot build route");
            }

            var result = new List<RouteSettings>();
            var calc = new DistanceCalculator();

            var fleetStoreDistance = new List<RouteDistance>();
            foreach (var fleet in fleets)
            {
                foreach (var store in stores)
                {
                    fleetStoreDistance.Add(new RouteDistance
                                               {
                                                   LocationFrom = fleet.AddressLine,
                                                   LocationFromAddress = fleet,
                                                   LocationTo = store.AddressLine,
                                                   LocationToAddress = store,
                                                   Distance = calc.Calculate(fleet.Coordinates, store.Coordinates)
                                               });
                }
            }

            // get distance from client to store
            var storeToClientDistances = new LinkedList<RouteDistance>();
            foreach (var client in clientAddresses)
            {
                foreach (var store in stores)
                {
                    storeToClientDistances.AddLast(
                        new RouteDistance
                            {
                                LocationFrom = store.AddressLine,
                                LocationFromAddress =
                                    new Address { AddressLine = store.AddressLine, Coordinates = store.Coordinates },
                                LocationTo = client.AddressLine,
                                LocationToAddress =
                                    new Address { AddressLine = client.AddressLine, Coordinates = client.Coordinates },
                                Distance = calc.Calculate(client.Coordinates, store.Coordinates)
                            });
                }

                var nearestStore = storeToClientDistances.OrderBy(x => x.Distance).FirstOrDefault();
                var nearestFleet = fleetStoreDistance.FirstOrDefault(
                    x => x.LocationTo.Equals(nearestStore.LocationFrom, StringComparison.InvariantCultureIgnoreCase));

                result.Add(this.BuildRouteSettings(calc, nearestFleet, nearestStore));

                storeToClientDistances.Clear();
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
            var stores = (await this.storeFinder.GetStoresToServe()).ToList();
            var drones = (await this.droneService.GetAvailableDrones()).ToList();

            var clientAddress = await this.locationFinder.GetAddressCoordinates(address);

            if (!stores.AnySafe() || !drones.AnySafe() || clientAddress == null)
            {
                return this.Ok("cannot build route");
            }

            var calc = new DistanceCalculator();

            var storeClientDistance = new List<RouteDistance>();
            foreach (var store in stores)
            {
                storeClientDistance.Add(new RouteDistance
                                            {
                                                LocationFrom = store.AddressLine,
                                                LocationFromAddress = store,
                                                LocationTo = clientAddress.AddressLine,
                                                LocationToAddress = clientAddress,
                                                Distance = calc.Calculate(store.Coordinates, clientAddress.Coordinates)
                                            });
            }

            var nearestStore = storeClientDistance.OrderBy(x => x.Distance).FirstOrDefault();

            var droneStoreDistance = new List<RouteDistance>();
            foreach (var fleet in drones)
            {
                foreach (var store in stores)
                {
                    if (store.AddressLine.Equals(nearestStore.LocationFrom, StringComparison.InvariantCultureIgnoreCase))
                    {
                        droneStoreDistance.Add(
                            new RouteDistance
                                {
                                    LocationFrom = fleet.AddressLine,
                                    LocationFromAddress = fleet,
                                    LocationTo = store.AddressLine,
                                    LocationToAddress = store,
                                    Distance = calc.Calculate(fleet.Coordinates, store.Coordinates)
                                });
                        break;
                    }
                }
            }

            var nearestFleet = droneStoreDistance.OrderBy(x => x.Distance).FirstOrDefault();

            return this.Ok(this.BuildRouteSettings(calc, nearestFleet, nearestStore));
        }

        /// <summary>
        /// The build route settings.
        /// </summary>
        /// <param name="calc">
        /// The calc.
        /// </param>
        /// <param name="fleetToStore">
        /// The fleet to store.
        /// </param>
        /// <param name="storeToClient">
        /// The store to client.
        /// </param>
        /// <returns>
        /// The <see cref="RouteSettings"/>.
        /// </returns>
        private RouteSettings BuildRouteSettings(
            DistanceCalculator calc,
            RouteDistance fleetToStore,
            RouteDistance storeToClient)
        {
            var result = new RouteSettings();

            result.ClientLocation = storeToClient.LocationToAddress;
            result.StoreLocation = storeToClient.LocationFromAddress;
            result.DroneLocation = fleetToStore.LocationFromAddress;

            result.DistanceToClient = fleetToStore.Distance + storeToClient.Distance;
            result.DroneFlyDistance = result.DistanceToClient + calc.Calculate(
                                          result.ClientLocation.Coordinates,
                                          result.DroneLocation.Coordinates);

            result.ClientWaitingTimeSec = (result.DistanceToClient * 1000) / 16.6667d;
            result.DroneFlyTimeSec = (result.DroneFlyDistance * 1000) / 16.6667d;

            result.ClientWaitingTimeMin = result.ClientWaitingTimeSec / 60;
            result.DroneFlyTimeMin = result.DroneFlyTimeSec / 60;

            return result;
        }
    }
}
