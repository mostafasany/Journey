using System;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace Abstractions.Forms
{
    public class LocationService : ILocationService
    {
        private const double PIx = Math.PI;

        public event EventHandler<Location> LocationObtained;

        public double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2)
        {
            try
            {
                double R = 6371; // km
                double dLat = Radians(lat2 - lat1);
                double dLon = Radians(lon2 - lon1);
                lat1 = Radians(lat1);
                lat2 = Radians(lat2);

                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                           Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                double d = R * c;

                return d;
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }

        public async Task<Location> ObtainMyLocationAsync()
        {
            try
            {
                IGeolocator locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                Position position = null;
                Task<Position> task = Task.Run(() => locator.GetPositionAsync(TimeSpan.FromSeconds(2), null, true));
                if (task.Wait(TimeSpan.FromSeconds(2)))
                    position = task.Result;
                if (position == null)
                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(1), null, true);

                //CrossExternalMaps.Current.NavigateTo("teste", latitude, longitude);
                //var locator = CrossGeolocator.Current;
                //locator.DesiredAccuracy = 50;

                var loc = new Location
                {
                    Lat = position.Latitude,
                    Lng = position.Longitude
                };

                LocationObtained?.Invoke(this, loc);
                return loc;
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
        private double Radians(double x) => x * PIx / 180;
    }
}