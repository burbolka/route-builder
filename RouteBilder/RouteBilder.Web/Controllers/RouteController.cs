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
    using RouteBuilder.Web.Helpers;
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
            var resultTask = Task<List<RouteSettings>>.Factory.StartNew(() =>
            {
                // get stores
                var stores = this.storeFinder.GetStoresToServe().ToList();
                var fleets = this.droneService.GetDroneFleets().ToList();
                var clientAddresses = clientLocations.Value;

                var result = new List<RouteSettings>();

                if (!stores.AnySafe() || !fleets.AnySafe() || !clientAddresses.AnySafe())
                {
                    return result;
                }

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

                    var availableDrones = this.droneService.GetAvailableDrones();
                    var nearestStore = storeToClientDistances.OrderBy(x => x.Distance).FirstOrDefault();

                    var nearestFleet = fleetStoreDistance
                        .Where(x => x.LocationTo.Equals(nearestStore.LocationFrom, StringComparison.InvariantCultureIgnoreCase))
                        .OrderBy(x => x.Distance)
                        .FirstOrDefault();

                    var availableDroneNearby = availableDrones.FirstOrDefault(x => x.AddressLine.Equals(
                        nearestFleet.LocationFrom,
                        StringComparison.InvariantCultureIgnoreCase));

                    if (availableDroneNearby == null)
                    {
                        var availableDronesDistance = fleetStoreDistance
                            .Where(x => !x.LocationFrom.Equals(nearestFleet.LocationFrom, StringComparison.InvariantCultureIgnoreCase))
                            .ToList();

                        nearestFleet = availableDronesDistance
                            .OrderBy(x => x.Distance)
                            .FirstOrDefault();
                    }

                    result.Add(BuildRouteHelper.BuildRouteSettings(calc, nearestFleet, nearestStore));

                    storeToClientDistances.Clear();
                }

                return result;
            });
            
            return this.Ok(await resultTask);
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
            var resultTask = Task<RouteSettings>.Factory.StartNew(
                () =>
                    {
                        // get stores
                        var stores = this.storeFinder.GetStoresToServe().ToList();
                        var drones = this.droneService.GetAvailableDrones().ToList();

                        var clientAddress = this.locationFinder.GetAddressCoordinates(address);

                        if (!stores.AnySafe() || !drones.AnySafe() || clientAddress == null)
                        {
                            return null;
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

                        var nearestFleet = droneStoreDistance
                            .Where(x => x.LocationTo.Equals(nearestStore.LocationFrom, StringComparison.CurrentCultureIgnoreCase))
                            .OrderBy(x => x.Distance)
                            .FirstOrDefault();

                        return BuildRouteHelper.BuildRouteSettings(calc, nearestFleet, nearestStore);
                    });

            return this.Ok(await resultTask);
        }
    }
}
