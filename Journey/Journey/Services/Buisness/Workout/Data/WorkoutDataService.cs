using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Services.Azure;
using Journey.Services.Buisness.Workout.Dto;
using Journey.Services.Buisness.Workout.Translators;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Workout.Data
{
    public class WorkoutDataService : IWorkoutDataService
    {
        private readonly IMobileServiceTable<AzureAccountWorkouts> _azureAccountWorkout;
        private readonly IMobileServiceTable<AzureWorkout> _azureWorkout;
        private IEnumerable<IGrouping<string, AzureWorkout>> _workoutGroupCategories;

        public WorkoutDataService(IAzureService azureService)
        {
            MobileServiceClient client = azureService.CreateOrGetAzureClient();
            _azureWorkout = client.GetTable<AzureWorkout>();
            _azureAccountWorkout = client.GetTable<AzureAccountWorkouts>();
        }

        public async Task<List<Models.Workout>> GetLogWorkoutsAsync()
        {
            try
            {
                IEnumerable<IGrouping<string, AzureAccountWorkouts>> accountWorkoutGroups = await GetAccountWorkoutAsync();

                await SetWorkoutGroups();

                IEnumerable<IGrouping<string, AzureWorkout>> workouts = _workoutGroupCategories.Where(a => a.Key == null);
                List<Models.Workout> workoutDto = WorkoutDataTranslator.TranslateWorkouts(workouts.FirstOrDefault().ToList());
                foreach (Models.Workout item in workoutDto)
                {
                    IEnumerable<IGrouping<string, AzureWorkout>> group = _workoutGroupCategories.Where(a => a.Key == item.Id);
                    if (group == null || group?.FirstOrDefault() == null)
                        continue;

                    List<Models.Workout> groupWorkoutDto = WorkoutDataTranslator.TranslateWorkouts(group?.FirstOrDefault()?.ToList());

                    groupWorkoutDto =  SetAccountMaxWeightWorkout(accountWorkoutGroups, groupWorkoutDto);

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
                AzureAccountWorkouts workoutDto = WorkoutDataTranslator.TranslateLogWorkout(workout);
                workoutDto.Account = _azureAccountWorkout.MobileServiceClient.CurrentUser.UserId;
                await _azureAccountWorkout.InsertAsync(workoutDto);
                return true;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        private async Task<IEnumerable<IGrouping<string, AzureAccountWorkouts>>> GetAccountWorkoutAsync()
        {
            string account = _azureAccountWorkout.MobileServiceClient.CurrentUser.UserId;

            IEnumerable<AzureAccountWorkouts> accountWorkouts = await _azureAccountWorkout.Where(a => a.Account == account)
                .ToListAsync();
            IEnumerable<IGrouping<string, AzureAccountWorkouts>> groupWorkout = accountWorkouts.GroupBy(a => a.Workout);
            return groupWorkout;
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


                AzureWorkout workoutDto = WorkoutDataTranslator.TranslateWorkout(workout);
                await _azureWorkout.InsertAsync(workoutDto);
                workout = WorkoutDataTranslator.TranslateWorkout(workoutDto);
                return workout;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        private List<Models.Workout> SetAccountMaxWeightWorkout(IEnumerable<IGrouping<string, AzureAccountWorkouts>> accountWorkoutGroups,
            List<Models.Workout> groupWorkoutDto)
        {
            if (accountWorkoutGroups != null && accountWorkoutGroups.Any())
                foreach (Models.Workout accountGroup in groupWorkoutDto)
                {
                    List<AzureAccountWorkouts> grp = accountWorkoutGroups.Where(a => a.Key == accountGroup.Id).FirstOrDefault()
                        .OrderByDescending(a => a.Weight).ToList();

                    var maxWeightItem = grp.First();

                    accountGroup.MaxWeight = maxWeightItem.Weight;
                    accountGroup.MaxRips = maxWeightItem.Rips;
                }

            return groupWorkoutDto;
        }

        private async Task SetWorkoutGroups()
        {
            if (_workoutGroupCategories == null)
            {
                List<AzureWorkout> workoutCategories = await _azureWorkout.ToListAsync();
                _workoutGroupCategories = workoutCategories.GroupBy(a => a.Parent);
            }
        }
    }
}