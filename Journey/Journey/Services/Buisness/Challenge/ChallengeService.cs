using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account.Data;
using Tawasol.Services.Data;

namespace Tawasol.Services
{
    public class ChallengeService : IChallengeService
    {
        private readonly  IChallengeDataService challengeDataService;
        private readonly IAccountDataService accountDataService;
        public ChallengeService(IChallengeDataService _challengeDataService,IAccountDataService _accountDataService)
        {
            challengeDataService = _challengeDataService;
            accountDataService = _accountDataService;
        }

       
        public async Task<Challenge> GetChallengeAsync(string challengeId)
        {
            try
            {
                var challengeDTO = await challengeDataService.GetChallengeAsync(challengeId);
                return challengeDTO;
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public async Task<Challenge> SaveCurrentChallengeAsync(Challenge challenge)
        {
            try
            {
                var challengeDTO = await challengeDataService.AddChallengeAsync(challenge);
                var account = challenge.ChallengeAccounts.FirstOrDefault();
                account.ChallengeId = challengeDTO.Id;
                await  accountDataService.AddUpdateAccountAsync(account,false);
                return challengeDTO;
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Challenge> GetAccountChallengeAsync()
        {
            try
            {
                var challengeDTO = await challengeDataService.GetAccountChallengeAsync();
                return challengeDTO;
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}
