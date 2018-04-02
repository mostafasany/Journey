using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Post;

namespace Journey.Services.Buisness.Measurment
{
    public interface IWorkoutService
    {
        Task<List<Models.Workout>> GetWorkoutCategoriesAsync();
        Task<List<Models.Workout>> GetWorkoutSubCategoriesAsync(string categroy);
    }
}