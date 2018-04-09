using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journey.Services.Buisness.Workout.Data
{
    public interface IWorkoutDataService
    {
        Task<List<Models.Workout>> GetLogWorkoutsAsync();
        Task<bool> LogWorkout(Models.Workout workout);
    }
}