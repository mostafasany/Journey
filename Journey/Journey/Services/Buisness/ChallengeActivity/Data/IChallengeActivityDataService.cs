using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journey.Services.Buisness.ChallengeActivity.Data
{
    public interface IChallengeActivityDataService
    {
        Task<Models.Challenge.ChallengeActivityLog> AddActivityAsync(Models.Challenge.ChallengeActivityLog log);
        Task<bool> DeleteActivityAsync(Models.Challenge.ChallengeActivityLog log);
        Task<List<Models.Challenge.ChallengeActivityLog>> GetActivitsAsync(string challengeId, int page, int size);
    }
}