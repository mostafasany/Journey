using System;
using System.Threading.Tasks;
using Abstractions.Models;

namespace Abstractions.Services.Contracts
{
    public interface ILocationService
    {
        Task<Location> ObtainMyLocationAsync();
        double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2);
        event EventHandler<Location> LocationObtained;
    }
}