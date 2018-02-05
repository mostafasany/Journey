using System;
using System.Threading.Tasks;
using Abstractions.Models;

namespace Abstractions.Services.Contracts
{
    public interface ILocationService
    {
        Task<Location> ObtainMyLocationAsync();
        event EventHandler<Location> LocationObtained;
    }
}