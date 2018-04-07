using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Services.Azure;
using Microsoft.WindowsAzure.MobileServices;
using System.Linq;
namespace Journey.Services.Buisness.Workout.Data
{
    public class WorkoutDataService : IWorkoutDataService
    {
        private readonly IMobileServiceTable<Dto.AzureWorkout> _azureWorkout;
        private readonly IMobileServiceTable<Dto.AzureAccountWorkouts> _azureAccountWorkout;
        private readonly MobileServiceClient _client;
        IEnumerable<IGrouping<string, Dto.AzureWorkout>> _WorkoutGroupCategories;
        IEnumerable<Dto.AzureAccountWorkouts> _AccountWorkouts;
        public WorkoutDataService(IAzureService azureService)
        {
            _client = azureService.CreateOrGetAzureClient();
            _azureWorkout = _client.GetTable<Dto.AzureWorkout>();
            _azureAccountWorkout = _client.GetTable<Dto.AzureAccountWorkouts>();
        }

        private async Task<IEnumerable<IGrouping<string, Dto.AzureAccountWorkouts>>> GetAccountWorkoutAsync()
        {
            var account = _azureAccountWorkout.MobileServiceClient.CurrentUser.UserId;

            IEnumerable<Dto.AzureAccountWorkouts> accountWorkouts = await _azureAccountWorkout.Where(a => a.Account == account)
                                                               .ToListAsync();
            var groupWorkout = accountWorkouts.GroupBy(a => a.Workout);
            return groupWorkout;
        }

        private async Task SetWorkoutGroups()
        {

            if (_WorkoutGroupCategories == null)
            {
                var workoutCategories = await _azureWorkout.ToListAsync();
                _WorkoutGroupCategories = workoutCategories.GroupBy(a => a.Parent);
            }
        }

        private async Task<List<Models.Workout>> SetAccountMaxWeightWorkout(IEnumerable<IGrouping<string, Dto.AzureAccountWorkouts>> accountWorkoutGroups,
                                                      List<Models.Workout> groupWorkoutDto)
        {
            if (accountWorkoutGroups != null && accountWorkoutGroups.Any())
            {
                foreach (var accountGroup in groupWorkoutDto)
                {
                    var grp = accountWorkoutGroups.Where(a => a.Key == accountGroup.Id).FirstOrDefault()
                                        .OrderByDescending(a => a.Weight).ToList();
                    accountGroup.MaxWeight ="Max: "+ grp.First().Weight;
                    accountGroup.MaxRips = "Max: " + grp.First().Rips;
                }
            }
            return groupWorkoutDto;
        }

        public async Task<List<Models.Workout>> GetLogWorkoutsAsync()
        {
            try
            {
                var accountWorkoutGroups = await GetAccountWorkoutAsync();

                await SetWorkoutGroups();

                var workouts = _WorkoutGroupCategories.Where(a => a.Key == null);
                var workoutDto = Translators.WorkoutDataTranslator.TranslateWorkouts(workouts.FirstOrDefault().ToList());
                foreach (var item in workoutDto)
                {
                    var group = _WorkoutGroupCategories.Where(a => a.Key == item.Id);
                    if (group == null || group?.FirstOrDefault() == null)
                        continue;

                    var groupWorkoutDto = Translators.WorkoutDataTranslator.TranslateWorkouts(group?.FirstOrDefault()?.ToList());

                    groupWorkoutDto = await SetAccountMaxWeightWorkout(accountWorkoutGroups, groupWorkoutDto);

                    item.Workouts = groupWorkoutDto;
                }
                return workoutDto;

            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<bool> LogWorkout(Models.Workout workout)
        {
            try
            {
                Dto.AzureAccountWorkouts workoutDto = Translators.WorkoutDataTranslator.TranslateLogWorkout(workout);
                workoutDto.Account = _azureAccountWorkout.MobileServiceClient.CurrentUser.UserId;
                await _azureAccountWorkout.InsertAsync(workoutDto);
                return true;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        private async Task<Models.Workout> MockWorkout(Models.Workout workout)
        {
            try
            {
                //List<Models.Workout> chestworkouts = new List<Models.Workout>();
                //chestworkouts.Add(new Models.Workout { Name = "Barbell Bench Press", Image = "https://bit.ly/2EaQthN", Parent = "6d79ade3-4c72-45b5-9fd7-cb839d04f822" });
                //chestworkouts.Add(new Models.Workout { Name = "Flat Bench Dumbbell Press", Image = "https://bit.ly/2EcjsS5", Parent = "6d79ade3-4c72-45b5-9fd7-cb839d04f822" });
                //chestworkouts.Add(new Models.Workout { Name = "Low-Incline Barbell Bench Press", Image = "https://bit.ly/2uAhKum", Parent = "6d79ade3-4c72-45b5-9fd7-cb839d04f822" });
                //chestworkouts.Add(new Models.Workout { Name = "Machine Decline Press", Image = "https://bit.ly/2H14Wjf", Parent = "6d79ade3-4c72-45b5-9fd7-cb839d04f822" });
                //chestworkouts.Add(new Models.Workout { Name = "Seated Machine Chest Press", Image = "https://bit.ly/2pYf842", Parent = "6d79ade3-4c72-45b5-9fd7-cb839d04f822" });
                //chestworkouts.Add(new Models.Workout { Name = "Incline Dumbbell Press", Image = "https://bit.ly/2GN78xd", Parent = "6d79ade3-4c72-45b5-9fd7-cb839d04f822" });


                //List<Models.Workout> workouts = new List<Models.Workout>();
                //workouts.Add(new Models.Workout { Name = "Chest", Image = "https://bit.ly/2EaQthN", Workouts = chestworkouts });
                //workouts.Add(new Models.Workout { Name = "Back", Image = "https://bit.ly/2EcjsS5" });
                //workouts.Add(new Models.Workout { Name = "Biceps", Image = "https://bit.ly/2uAhKum" });
                //workouts.Add(new Models.Workout { Name = "TriSepcs", Image = "https://bit.ly/2H14Wjf" });
                //workouts.Add(new Models.Workout { Name = "Leg", Image = "https://bit.ly/2pYf842" });
                //workouts.Add(new Models.Workout { Name = "Warming Up", Image = "https://bit.ly/2GN78xd" });


                Dto.AzureWorkout workoutDto = Translators.WorkoutDataTranslator.TranslateWorkout(workout);
                await _azureWorkout.InsertAsync(workoutDto);
                workout = Translators.WorkoutDataTranslator.TranslateWorkout(workoutDto);
                return workout;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }
    }
}