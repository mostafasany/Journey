using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Challenge;

namespace Journey.Services.Buisness.ChallengeActivity
{
    public interface IChallengeActivityService
    {
        Task<ChallengeActivityLog> AddActivityAsync(ChallengeActivityLog log);
        Task<ChallengeActivityLog> UpdateActivityAsync(ChallengeActivityLog log);
        Task<bool> DeleteActivityAsync(ChallengeActivityLog log);
        Task<List<ChallengeActivityLog>> GetActivitsAsync(string challengeId);
        Task<List<ObservableChallengeProgressGroupCollection<AccountChallengeProgress>>> GetChallengePorgessAsync(string challengeId);
    }
}