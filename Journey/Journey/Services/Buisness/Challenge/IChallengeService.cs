using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journey.Services.Buisness.Challenge
{
    public interface IChallengeService
    {
        Task<Models.Challenge.Challenge> AddChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> ApproveChallengeAsync(string challenge);
        Task<Models.Challenge.Challenge> EditChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> EndChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId);
        Task<List<Models.Challenge.Challenge>> GetChallengeRequestsAsync();
        Task<bool> HasChallengeAsync(string friend);
    }
}