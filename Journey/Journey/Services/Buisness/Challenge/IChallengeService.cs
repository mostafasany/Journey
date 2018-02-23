using System.Threading.Tasks;

namespace Journey.Services.Buisness.Challenge
{
    public interface IChallengeService
    {
        Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId);
        Task<Models.Challenge.Challenge> SaveCurrentChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> GetAccountChallengeAsync();
    }
}