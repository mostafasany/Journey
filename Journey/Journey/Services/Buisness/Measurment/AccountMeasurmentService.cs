using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Post;
using Tawasol.Services.Data;

namespace Journey.Services.Buisness.Measurment
{
    public class AccountMeasurmentService : IAccountMeasurmentService
    {
        private readonly IAccountMeasurmentDataService accountDataService;

        public AccountMeasurmentService(IAccountMeasurmentDataService _accountDataService)
        {
            accountDataService = _accountDataService;
        }

        public event ScaleMeasurmentsChangedEventHandler ScaleMeasurmentsChangedHandler;

        public async Task<List<ScaleMeasurment>> GetMeasurmentsAsync(bool sync = false)
        {
            try
            {
                var measuremnts = new List<ScaleMeasurment>();
                measuremnts = await accountDataService.GetAccountMeasurmentAsync(sync);
                return measuremnts;
            }
            catch (Exception ex)
            {
                throw new BuisnessException(ex.Message, ex);
            }
        }


        public async Task UpdateScaleMeasurments(List<ScaleMeasurment> measurments)
        {
            measurments = await accountDataService.AddUpdateAccountMeasurmentAsync(measurments);
            ScaleMeasurmentsChangedHandler?.Invoke(this, new ScaleMeasurmentsChangedArgs {Measuremnts = measurments});
        }
    }
}