using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IHealthService
    {
        Task<bool> Authenticate();

        event HealthDataEventHandler HealthDataChanged;
        Task GetAgeAsync();
        Task GetWeightAsync();
        Task GetHeightAsync();
        Task GetCaloriesAsync();
        Task GetStepsAsync();
    }

    public delegate void HealthDataEventHandler(object sender, HealthDataEventArgs e);

    public class HealthDataEventArgs : EventArgs
    {
        public Dictionary<string, string> Data { get; set; }
    }
}