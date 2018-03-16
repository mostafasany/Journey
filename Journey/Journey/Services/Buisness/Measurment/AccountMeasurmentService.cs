using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Post;
using Journey.Services.Buisness.Measurment.Data;

namespace Journey.Services.Buisness.Measurment
{
    public class AccountMeasurmentService : IAccountMeasurmentService
    {
        private readonly IAccountMeasurmentDataService _accountDataService;

        public AccountMeasurmentService(IAccountMeasurmentDataService accountDataService) => _accountDataService = accountDataService;

        public async Task<List<ScaleMeasurment>> GetMeasurmentsAsync(bool sync = false)
        {
            try
            {
                List<ScaleMeasurment> measuremnts = await _accountDataService.GetAccountMeasurmentAsync(sync);
                return measuremnts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<ScaleMeasurment>> UpdateScaleMeasurments(List<ScaleMeasurment> measurments)
        {
            try
            {
                measurments = await _accountDataService.AddUpdateAccountMeasurmentAsync(measurments);
                return measurments;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}