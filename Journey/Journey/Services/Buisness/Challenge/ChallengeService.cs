using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Account;
using Journey.Models.Challenge;
using Tawasol.Services.Data;

namespace Tawasol.Services
{
    public class ChallengeService : IChallengeService
    {
        IChallengeDataService challengeDataService;
        public ChallengeService(IChallengeDataService _challengeDataService)
        {
            challengeDataService = _challengeDataService;
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
