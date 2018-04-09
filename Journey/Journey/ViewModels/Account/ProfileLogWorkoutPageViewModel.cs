using System;
using System.Collections.Generic;
using System.Linq;
using Journey.Models;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Notification;
using Journey.Services.Buisness.Workout;
using Journey.ViewModels.Wall;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ProfileLogWorkoutPageViewModel : ProfilePageViewModel, INavigationAware
    {
        private readonly IWorkoutService _workoutService;

        public ProfileLogWorkoutPageViewModel(IUnityContainer container, IAccountService accountService, INotificationService notificationService,
            IWorkoutService workoutService) :
            base(container, accountService, notificationService) => _workoutService = workoutService;

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                ClearTabSelection();
                SecondTabSelected = "#f1f1f1";

                if (parameters.GetNavigationMode() == NavigationMode.New)
                    Intialize();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        #endregion

        #region Properties

        private List<WorkoutLogViewModel> _workoutCategories;

        public List<WorkoutLogViewModel> WorkoutCategories
        {
            get => _workoutCategories;
            set => SetProperty(ref _workoutCategories, value);
        }

        private WorkoutLogViewModel _selectedWorkoutCategory;

        public WorkoutLogViewModel SelectedWorkoutCategory
        {
            get => _selectedWorkoutCategory;
            set => SetProperty(ref _selectedWorkoutCategory, value);
        }


        private List<WorkoutLogViewModel> _workoutSubCategories;

        public List<WorkoutLogViewModel> WorkoutSubCategories
        {
            get => _workoutSubCategories;
            set => SetProperty(ref _workoutSubCategories, value);
        }

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                List<Workout> categories = await _workoutService.GetLogWorkoutsAsync();
                WorkoutCategories = new List<WorkoutLogViewModel>();
                if (categories.Any())
                {
                    foreach (Workout item in categories) WorkoutCategories.Add(ConvertToWorkoutLogViewModel(item));
                    SelectedWorkoutCategory = WorkoutCategories.FirstOrDefault();
                    OnSelectedWorkoutCategory(SelectedWorkoutCategory);
                }

                base.Intialize(sync);
            }
            catch (Exception e)
            {
                ExceptionService.HandleAndShowDialog(e);
            }
            finally
            {
                HideProgress();
            }
        }

        private WorkoutLogViewModel ConvertToWorkoutLogViewModel(Workout item) => new WorkoutLogViewModel(Container)
        {
            Workout = item
        };

        protected override void Cleanup()
        {
            try
            {
                //Here Cleanup any resources
                base.Cleanup();
            }
            catch (Exception e)
            {
                ExceptionService.HandleAndShowDialog(e);
            }
        }

        #endregion

        #region Commands

        #region OnRefreshPostsCommand

        public DelegateCommand<WorkoutLogViewModel> OnSelectedWorkoutCategoryCommand => new DelegateCommand<WorkoutLogViewModel>(OnSelectedWorkoutCategory);


        private void OnSelectedWorkoutCategory(WorkoutLogViewModel value)
        {
            WorkoutSubCategories = new List<WorkoutLogViewModel>();
            if (value?.Workout?.Workouts == null)
                return;

            foreach (Workout item in value.Workout.Workouts) WorkoutSubCategories.Add(ConvertToWorkoutLogViewModel(item));
        }

        #endregion

        #endregion
    }
}