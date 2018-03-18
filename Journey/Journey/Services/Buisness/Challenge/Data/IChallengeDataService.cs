using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Challenge;

namespace Journey.Services.Buisness.Challenge.Data
{
    public interface IChallengeDataService
    {
        Task<Models.Challenge.Challenge> AddChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> CheckAccountHasChallengeAsync();
        Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId);
        Task<List<ChallengeProgress>> GetChallengePorgessAsync(string challengeId);
        Task<Models.Challenge.Challenge> UpdateChallengeAsync(Models.Challenge.Challenge challenge);
        Task<List<ChallengeActivityLog>> GetChallengeActivityLogAsync(string challengeId);
    }
}