using System;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Models;
using Abstractions.Services.Contracts;

namespace Journey.Services.Forms
{
    internal class LocationService : ILocationService
    {
        private const double PIx = Math.PI;
        public event EventHandler<Location> LocationObtained;

        public double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2)
        {
            try
            {
                double R = 6371; // km
                var dLat = Radians(lat2 - lat1);
                var dLon = Radians(lon2 - lon1);
                lat1 = Radians(lat1);
                lat2 = Radians(lat2);

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                var d = R * c;

                return d;
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }

        public Task<Location> ObtainMyLocationAsync()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }

        /// <summary>
        ///     Convert degrees to Radians
        /// </summary>
        /// <param name="x">Degrees</param>
        /// <returns>The equivalent in radians</returns>
        private double Radians(double x)
        {
            return x * PIx / 180;
        }
    }
}