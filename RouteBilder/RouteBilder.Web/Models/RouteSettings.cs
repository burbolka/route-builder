// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteSettings.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the RouteSettings type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Web.Models
{
    using System.Collections.Generic;

    using RouteBuilder.Common.Interfaces.Models.Address;

    /// <summary>
    /// The route settings.
    /// </summary>
    public class RouteSettings
    {
        /// <summary>
        /// Gets or sets the client location.
        /// </summary>
        public IAddressItem ClientLocation { get; set; }

        /// <summary>
        /// Gets or sets the store location.
        /// </summary>
        public IAddressItem StoreLocation { get; set; }

        /// <summary>
        /// Gets or sets the drone location.
        /// </summary>
        public IAddressItem DroneLocation { get; set; }

        /// <summary>
        /// Gets or sets the distance to client.
        /// </summary>
        public double DistanceToClient { get; set; }

        /// <summary>
        /// Gets or sets the drone fly distance.
        /// </summary>
        public double DroneFlyDistance { get; set; }

        /// <summary>
        /// Gets or sets the client waiting time minutes.
        /// </summary>
        public double ClientWaitingTimeMin { get; set; }

        /// <summary>
        /// Gets or sets the drone fly time minutes.
        /// </summary>
        public double DroneFlyTimeMin { get; set; }

        /// <summary>
        /// Gets or sets the client waiting time sec.
        /// </summary>
        public double ClientWaitingTimeSec { get; set; }

        /// <summary>
        /// Gets or sets the drone fly time sec.
        /// </summary>
        public double DroneFlyTimeSec { get; set; }
    }
}
