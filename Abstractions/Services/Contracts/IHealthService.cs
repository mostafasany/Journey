using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IHealthService
    {
        Task<bool> Authenticate();
        Task GetAgeAsync();
        Task GetCaloriesAsync();
        Task GetHeightAsync();
        Task GetStepsAsync();
        Task GetWeightAsync();

        event HealthDataEventHandler HealthDataChanged;
    }

    public delegate void HealthDataEventHandler(object sender, HealthDataEventArgs e);

    public class HealthDataEventArgs : EventArgs
    {
        public Dictionary<string, string> Data { get; set; }
    }
}