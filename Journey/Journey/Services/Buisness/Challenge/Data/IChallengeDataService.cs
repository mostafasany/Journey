using System.Threading.Tasks;

namespace Journey.Services.Buisness.Challenge.Data
{
    public interface IChallengeDataService
    {
        Task<Models.Challenge.Challenge> AddChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> CheckAccountHasChallengeAsync();
        Task<Models.Challenge.Challenge> EditChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId);
        Task<Models.Challenge.Challenge> UpdateChallengeAsync(Models.Challenge.Challenge challenge);
    }
}