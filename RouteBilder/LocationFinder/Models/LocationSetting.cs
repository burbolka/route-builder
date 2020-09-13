// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocationSetting.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the LocationSetting type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Services.LocationFinder.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RouteBuilder.Common.ExtensionMethods;

    /// <summary>
    /// The location setting.
    /// </summary>
    public class LocationSetting
    {
        /// <summary>
        /// Gets or sets the address line.
        /// </summary>
        public string AddressLine { get; set; }

        /// <summary>
        /// Gets or sets the coordinates.
        /// </summary>
        public Coordinates Coordinates { get; set; }

        /// <summary>
        /// The are equal.
        /// </summary>
        /// <param name="addressLine">
        /// The address line.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AreEqual(string addressLine)
        {
            return this.AddressLine.Equals(addressLine, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// The are equal.
        /// </summary>
        /// <param name="addressLine">
        /// The address line.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AreEqual(IEnumerable<string> addressLine)
        {
            return addressLine.AnySafe(x => x.Equals(this.AddressLine, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
