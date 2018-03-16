using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Post;

namespace Journey.Services.Buisness.Measurment.Data
{
    public interface IAccountMeasurmentDataService
    {
        Task<List<ScaleMeasurment>> AddUpdateAccountMeasurmentAsync(List<ScaleMeasurment> accountMeasurments);
        Task<List<ScaleMeasurment>> GetAccountMeasurmentAsync(bool sync = false);
    }
}