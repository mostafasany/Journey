using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.Exceptions;

namespace Journey.Services.Buisness.Workout.Translators
{
    public static class WorkoutDataTranslator
    {

        public static Dto.AzureAccountWorkouts TranslateLogWorkout(Models.Workout workout)
        {
            try
            {
                var accountDto = new Dto.AzureAccountWorkouts();
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

        public static Dto.AzureWorkout TranslateWorkout(Models.Workout workout)
        {
            try
            {
                var accountDto = new Dto.AzureWorkout();
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

        public static List<Models.Workout> TranslateWorkouts(List<Dto.AzureWorkout> accounts)
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

        public static Models.Workout TranslateWorkout(Dto.AzureWorkout workout)
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

    }
}