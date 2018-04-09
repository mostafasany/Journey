using System;
using System.Windows.Input;
using Journey.Models;
using Prism.Commands;
using Unity;

namespace Journey.ViewModels.Wall
{
    public class WorkoutLogViewModel : PostBaseViewModel
    {
        private readonly Services.Buisness.Workout.IWorkoutService _workoutService;

        public WorkoutLogViewModel(IUnityContainer container) : base(container) => _workoutService = container.Resolve<Services.Buisness.Workout.IWorkoutService>();

        Workout _workout;

        public Workout Workout
        {
            get => _workout;
            set => SetProperty(ref _workout, value);
        }


        #region LogCommand

        private ICommand _logCommand;

        public ICommand LogCommand => _logCommand ??
                                               (_logCommand =
                                                new DelegateCommand(Log));

        private async void Log()
        {
            try
            {
                if (Workout.Rips == string.Empty && Workout.Weight == string.Empty)
                {
                    return;
                }

                await _workoutService.LogWorkout(Workout);
                Workout.Rips = Workout.Weight = string.Empty;
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion
    }
}