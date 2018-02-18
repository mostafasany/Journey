using System.Threading.Tasks;
using Journey.Models.Challenge;

namespace Tawasol.Services.Data
{
    public interface IChallengeDataService
    {
        Task<Challenge> AddChallengeAsync(Challenge challenge);
        Task<Challenge> GetChallengeAsync(string challengeId);
        Task<Challenge> GetAccountChallengeAsync();
    }
}
