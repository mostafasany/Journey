using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Models;
using Journey.Models.Challenge;

namespace Journey.Services.Buisness.ChallengeActivity
{
    public interface IChallengeActivityService
    {
        Task<ChallengeActivityLog> AddExerciseActivityAsync(Location location, string challenge = null);
        Task<ChallengeActivityLog> AddUpdateActivityAsync(ChallengeActivityLog log);
        Task<bool> DeleteActivityAsync(ChallengeActivityLog log);
        Task<List<ChallengeActivityLog>> GetAccountActivitiesAsync();
        Task<List<ChallengeActivityLog>> GetChallengeActivitiesAsync(string challengeId);
        Task<List<ObservableChallengeProgressGroupCollection<AccountChallengeProgress>>> GetChallengePorgessAsync(string challengeId);
        Task<Models.Challenge.Challenge> IsExercisingInChallengeWorkoutPlaceAsync(Location location);
    }
}