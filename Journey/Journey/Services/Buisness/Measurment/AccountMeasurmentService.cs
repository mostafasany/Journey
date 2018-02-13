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
        private readonly IAccountMeasurmentDataService _accountDataService;

        public AccountMeasurmentService(IAccountMeasurmentDataService accountDataService)
        {
            _accountDataService = accountDataService;
        }

      //  public event ScaleMeasurmentsChangedEventHandler ScaleMeasurmentsChangedHandler;

        public async Task<List<ScaleMeasurment>> GetMeasurmentsAsync(bool sync = false)
        {
            try
            {
                var measuremnts = await _accountDataService.GetAccountMeasurmentAsync(sync);
                return measuremnts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public async Task<List<ScaleMeasurment>> UpdateScaleMeasurments(List<ScaleMeasurment> measurments)
        {
            measurments = await _accountDataService.AddUpdateAccountMeasurmentAsync(measurments);
            return measurments;
            //  ScaleMeasurmentsChangedHandler?.Invoke(this, new ScaleMeasurmentsChangedArgs {Measuremnts = measurments});
        }
    }
}