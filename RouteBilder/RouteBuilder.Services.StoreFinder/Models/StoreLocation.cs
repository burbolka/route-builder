// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoreLocation.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the StoreLocation type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Services.StoreFinder.Models
{
    using System;

    /// <summary>
    /// The store location.
    /// </summary>
    public class StoreLocation
    {
        /// <summary>
        /// Gets or sets the address line.
        /// </summary>
        public string AddressLine { get; set; }

        /// <summary>
        /// Gets a value indicating whether has drones.
        /// </summary>
        public bool CanServe => this.MagicStore ? this.MagicStore : this.Rand.Next(1, 1000) < 500;

        /// <summary>
        /// Gets or sets a value indicating whether magic store.
        /// </summary>
        public bool MagicStore { get; set; }

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
