using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models;
using Journey.Models.Post;
using Journey.Services.Buisness.Measurment.Data;

namespace Journey.Services.Buisness.Measurment
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IAccountMeasurmentDataService _accountDataService;

        public WorkoutService(IAccountMeasurmentDataService accountDataService) => _accountDataService = accountDataService;

        public async Task<List<Workout>> GetWorkoutCategoriesAsync()
        {
            try
            {
                List<Workout> workouts = new List<Workout>();
                workouts.Add(new Workout { Title = "Chest", Image = "https://bit.ly/2EaQthN" });
                workouts.Add(new Workout { Title = "Back", Image = "https://bit.ly/2EcjsS5" });
                workouts.Add(new Workout { Title = "Biceps", Image = "https://bit.ly/2uAhKum" });
                workouts.Add(new Workout { Title = "TriSepcs", Image = "https://bit.ly/2H14Wjf" });
                workouts.Add(new Workout { Title = "Leg", Image = "https://bit.ly/2pYf842" });
                workouts.Add(new Workout { Title = "Warming Up", Image = "https://bit.ly/2GN78xd" });
                return workouts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<Workout>> GetWorkoutSubCategoriesAsync(string categroy)
        {
            try
            {
                List<Workout> workouts = new List<Workout>();
                workouts.Add(new Workout { Title = "Barbell Bench Press", Image = "https://bit.ly/2EaQthN" });
                workouts.Add(new Workout { Title = "Flat Bench Dumbbell Press", Image = "https://bit.ly/2EcjsS5" });
                workouts.Add(new Workout { Title = "Low-Incline Barbell Bench Press", Image = "https://bit.ly/2uAhKum" });
                workouts.Add(new Workout { Title = "Machine Decline Press", Image = "https://bit.ly/2H14Wjf" });
                workouts.Add(new Workout { Title = "Seated Machine Chest Press", Image = "https://bit.ly/2pYf842" });
                workouts.Add(new Workout { Title = "Incline Dumbbell Press", Image = "https://bit.ly/2GN78xd" });
                return workouts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}