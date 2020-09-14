// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DistanceCalculator.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the DistanceCalculator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Common.Helpers
{
    using System;

    using RouteBuilder.Common.Interfaces.Models.Address;

    /// <summary>
    /// The distance calculator.
    /// </summary>
    public class DistanceCalculator
    {
        /// <summary>
        /// The earth radius.
        /// </summary>
        private static double EarthRadius => 6371d;

        /// <summary>
        /// The calculate.
        /// </summary>
        /// <param name="address1">
        /// The address 1.
        /// </param>
        /// <param name="address2">
        /// The address 2.
        /// </param>
        /// <returns>
        /// The <see cref="decimal"/>.
        /// return distance in kilometers
        /// </returns>
        public double Calculate(IAddressCoordinates address1, IAddressCoordinates address2)
        {
            var arg1 = Math.Pow(Math.Sin((address2.Latitude - address1.Latitude) * 0.5), 2);
            var arg2 = Math.Pow(Math.Sin((address2.Longitude - address1.Longitude) * 0.5), 2);
            var argSqrt = Math.Sqrt(arg1 + (Math.Cos(address2.Latitude) * Math.Cos(address1.Latitude) * arg2));

            var result = 2d * EarthRadius * Math.Asin(argSqrt);
            return result;
        }
    }
}
