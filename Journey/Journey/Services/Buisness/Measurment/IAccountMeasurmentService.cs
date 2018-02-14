using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Post;

namespace Journey.Services.Buisness.Measurment
{
    public interface IAccountMeasurmentService
    {
        Task<List<ScaleMeasurment>> GetMeasurmentsAsync(bool sync = false);

        Task<List<ScaleMeasurment>> UpdateScaleMeasurments(List<ScaleMeasurment> measurments);
    }
}