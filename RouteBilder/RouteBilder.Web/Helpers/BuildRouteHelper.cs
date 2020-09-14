// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildRouteHelper.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the BuildRouteHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Web.Helpers
{
    using RouteBuilder.Common.Helpers;
    using RouteBuilder.Web.Models;

    /// <summary>
    /// The build route helper.
    /// </summary>
    public static class BuildRouteHelper
    {
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
        public static RouteSettings BuildRouteSettings(
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
        /// <param name="distanceToClient">
        /// The distance to client.
        /// </param>
        /// <returns>
        /// The <see cref="RouteSettings"/>.
        /// </returns>
        public static RouteSettings BuildRouteSettings(
            DistanceCalculator calc,
            RouteDistance fleetToStore,
            RouteDistance storeToClient,
            double distanceToClient)
        {
            var result = new RouteSettings();

            result.ClientLocation = storeToClient.LocationToAddress;
            result.StoreLocation = storeToClient.LocationFromAddress;
            result.DroneLocation = fleetToStore.LocationFromAddress;

            result.DistanceToClient = distanceToClient;
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
