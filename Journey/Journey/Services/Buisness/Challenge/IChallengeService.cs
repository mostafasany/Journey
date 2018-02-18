using System.Threading.Tasks;
using Journey.Models.Challenge;

namespace Tawasol.Services
{
    public interface IChallengeService
    {
        Task<Challenge> GetChallengeAsync(string challengeId);
        Task<Challenge> SaveCurrentChallengeAsync(Challenge challenge);
        Task<Challenge> GetAccountChallengeAsync();
    }
}
