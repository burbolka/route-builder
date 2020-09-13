// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DroneLocation.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the DroneLocation type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Services.DroneFinder.Models
{
    using System;

    using RouteBuilder.Common.Interfaces.Models.Address;

    /// <summary>
    /// The drone location.
    /// </summary>
    public class DroneLocation
    {
        /// <summary>
        /// Gets or sets the address line.
        /// </summary>
        public string AddressLine { get; set; }

        /// <summary>
        /// Gets a value indicating whether has drones.
        /// </summary>
        public bool HasDrones => this.UnlimitedDrones ? this.UnlimitedDrones : this.Rand.Next(1, 1000) < 500;

        /// <summary>
        /// Gets or sets a value indicating whether unlimited drones.
        /// </summary>
        public bool UnlimitedDrones { get; set; }

        /// <summary>
        /// Gets or sets the coordinates.
        /// </summary>
        public Coordinates Coordinates { get; set; }

        /// <summary>
        /// The rand.
        /// </summary>
        private Random Rand => new Random();
    }
}
