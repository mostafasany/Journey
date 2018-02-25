using System.Threading.Tasks;

namespace Journey.Services.Buisness.Challenge
{
    public interface IChallengeService
    {
        Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId);
        Task<Models.Challenge.Challenge> EndChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> AddChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> GetAccountChallengeAsync();
    }
}