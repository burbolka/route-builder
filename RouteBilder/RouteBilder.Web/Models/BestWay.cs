// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BestWay.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the BestWay type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Web.Models
{
    using RouteBuilder.Common.Interfaces.Models.Address;

    /// <summary>
    /// The best way.
    /// </summary>
    public class BestWay
    {
        /// <summary>
        /// Gets or sets the distance.
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Gets or sets the fleet index.
        /// </summary>
        public int FleetIndex { get; set; }

        /// <summary>
        /// Gets or sets the store index.
        /// </summary>
        public int StoreIndex { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        public IAddressItem Client { get; set; }
    }
}
