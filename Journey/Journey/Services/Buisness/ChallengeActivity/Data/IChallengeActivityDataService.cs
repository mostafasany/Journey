using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Challenge;

namespace Journey.Services.Buisness.ChallengeActivity.Data
{
    public interface IChallengeActivityDataService
    {
        Task<ChallengeActivityLog> AddActivityAsync(ChallengeActivityLog log);
        Task<bool> DeleteActivityAsync(ChallengeActivityLog log);
        Task<List<ChallengeActivityLog>> GetActivitsAsync(string challengeId);
        Task<ChallengeActivityLog> UpdateActivityAsync(ChallengeActivityLog log);
    }
}