using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.Exceptions;
using Journey.Services.Buisness.Workout.Dto;

namespace Journey.Services.Buisness.Workout.Translators
{
    public static class WorkoutDataTranslator
    {
        public static AzureAccountWorkouts TranslateLogWorkout(Models.Workout workout)
        {
            try
            {
                var workoutDto = new AzureAccountWorkouts();
                if (workout == null) return workoutDto;


                workoutDto.Rips = workout.Rips;
                workoutDto.Unit = workout.Unit;
                workoutDto.Weight = workout.Weight;
                workoutDto.Workout = workout.Id;
                return workoutDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Workout", ex.InnerException);
            }
        }

        public static AzureWorkout TranslateWorkout(Models.Workout workout)
        {
            try
            {
                var workoutDto = new AzureWorkout();
                if (workout == null) return workoutDto;

                workoutDto.Id = workout.Id;
                workoutDto.Image = workout.Image;
                workoutDto.Name = workout.Name;
                workoutDto.Parent = workout.Parent;

                return workoutDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Workout", ex.InnerException);
            }
        }

        public static Models.Workout TranslateWorkout(AzureWorkout workout)
        {
            try
            {
                var workoutDto = new Models.Workout();
                if (workout == null) return workoutDto;

                workoutDto.Id = workout.Id;
                workoutDto.Image = workout.Image;
                workoutDto.Name = workout.Name;
                workoutDto.Parent = workout.Parent;

                return workoutDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Workout", ex.InnerException);
            }
        }

        public static List<Models.Workout> TranslateWorkouts(List<AzureWorkout> workouts)
        {
            try
            {
                if (workouts == null)
                    return new List<Models.Workout>();
                return workouts.Select(TranslateWorkout).ToList();
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Workout", ex.InnerException);
            }
        }
    }
}