using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journey.Services.Buisness.Workout
{
    public interface IWorkoutService
    {
        Task<List<Models.Workout>> GetLogWorkoutsAsync();
        Task<bool> LogWorkoutAsync(Models.Workout workout);
    }
}