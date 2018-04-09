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
                var accountDto = new AzureAccountWorkouts();
                if (workout == null) return accountDto;


                accountDto.Rips = workout.Rips;
                accountDto.Unit = workout.Unit;
                accountDto.Weight = workout.Weight;
                accountDto.Workout = workout.Id;
                return accountDto;
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
                var accountDto = new AzureWorkout();
                if (workout == null) return accountDto;

                accountDto.Id = workout.Id;
                accountDto.Image = workout.Image;
                accountDto.Name = workout.Name;
                accountDto.Parent = workout.Parent;

                return accountDto;
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
                var accountDto = new Models.Workout();
                if (workout == null) return accountDto;

                accountDto.Id = workout.Id;
                accountDto.Image = workout.Image;
                accountDto.Name = workout.Name;
                accountDto.Parent = workout.Parent;

                return accountDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Workout", ex.InnerException);
            }
        }

        public static List<Models.Workout> TranslateWorkouts(List<AzureWorkout> accounts)
        {
            try
            {
                return accounts.Select(TranslateWorkout).ToList();
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Workout", ex.InnerException);
            }
        }
    }
}