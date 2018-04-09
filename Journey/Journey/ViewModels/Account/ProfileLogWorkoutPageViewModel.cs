using System;
using System.Collections.Generic;
using System.Linq;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.ChallengeActivity;
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class ProfileLogWorkoutPageViewModel : ProfilePageViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly Services.Buisness.Workout.IWorkoutService _workoutService;

        public ProfileLogWorkoutPageViewModel(IUnityContainer container, IAccountService accountService, INotificationService notificationService,
                                              Services.Buisness.Workout.IWorkoutService workoutService) :
            base(container, accountService, notificationService)
        {
            _accountService = accountService;
            _workoutService = workoutService;
        }

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

        List<Wall.WorkoutLogViewModel> _workoutCategories;

        public List<Wall.WorkoutLogViewModel> WorkoutCategories
        {
            get => _workoutCategories;
            set => SetProperty(ref _workoutCategories, value);
        }

        Wall.WorkoutLogViewModel _selectedWorkoutCategory;

        public Wall.WorkoutLogViewModel SelectedWorkoutCategory
        {
            get => _selectedWorkoutCategory;
            set => SetProperty(ref _selectedWorkoutCategory, value);
        }


        List<Wall.WorkoutLogViewModel> _workoutSubCategories;

        public List<Wall.WorkoutLogViewModel> WorkoutSubCategories
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
                var categories = await _workoutService.GetLogWorkoutsAsync();
                WorkoutCategories = new List<Wall.WorkoutLogViewModel>();
                if (categories.Any())
                {
                    foreach (var item in categories)
                    {
                        WorkoutCategories.Add(ConvertToWorkoutLogViewModel(item));
                    }
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

        private Wall.WorkoutLogViewModel ConvertToWorkoutLogViewModel(Models.Workout item)
        {
            return new Wall.WorkoutLogViewModel(Container)
            {
                Workout = item
            };
        }

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

        public DelegateCommand<Wall.WorkoutLogViewModel> OnSelectedWorkoutCategoryCommand => new DelegateCommand<Wall.WorkoutLogViewModel>(OnSelectedWorkoutCategory);


        private async void OnSelectedWorkoutCategory(Wall.WorkoutLogViewModel value)
        {
            WorkoutSubCategories = new List<Wall.WorkoutLogViewModel>();
            if (value?.Workout?.Workouts == null)
                return;

            foreach (var item in value.Workout.Workouts)
            {
                WorkoutSubCategories.Add(ConvertToWorkoutLogViewModel(item));
            }
        }

        #endregion

        #endregion
    }
}