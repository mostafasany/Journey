﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Exceptions;

namespace Journey.Services.Buisness.Workout
{
    public class WorkoutService : IWorkoutService
    {
        private readonly Data.IWorkoutDataService _workoutDataService;

        public WorkoutService(Data.IWorkoutDataService workoutDataService) => _workoutDataService = workoutDataService;

        public async Task<List<Models.Workout>> GetLogWorkoutsAsync()
        {
            try
            {
                List<Models.Workout> workouts = await _workoutDataService.GetLogWorkoutsAsync();
                return workouts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> LogWorkout(Models.Workout workout)
        {
            try
            {
                bool success = await _workoutDataService.LogWorkout(workout);
                return success;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}