using Journey.Models.Post;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tawasol.Services.Data
{
    public interface IAccountMeasurmentDataService
    {
        Task<List<ScaleMeasurment>> GetAccountMeasurmentAsync(bool sync = false);
        Task<List<ScaleMeasurment>> AddUpdateAccountMeasurmentAsync(List<ScaleMeasurment> accountMeasurments);
    }
}
