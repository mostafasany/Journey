using System;
using System.Collections.Generic;
using System.Linq;
using Journey.Models;
using Journey.Models.Challenge;
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
        private readonly IChallengeActivityService _challengeActivityService;
        private readonly Services.Buisness.Measurment.IWorkoutService _workoutService;

        public ProfileLogWorkoutPageViewModel(IUnityContainer container, IAccountService accountService, INotificationService notificationService,
                                              Services.Buisness.Measurment.IWorkoutService workoutService, IChallengeActivityService challengeActivityService) :
            base(container, accountService, notificationService)
        {
            _challengeActivityService = challengeActivityService;
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

        List<Workout> _workoutCategories;

        public List<Workout> WorkoutCategories
        {
            get => _workoutCategories;
            set => SetProperty(ref _workoutCategories, value);
        }

        Workout _selectedWorkoutCategory;

        public Workout SelectedWorkoutCategory
        {
            get => _selectedWorkoutCategory;
            set => SetProperty(ref _selectedWorkoutCategory, value);
        }


        List<Workout> _workoutSubCategories;

        public List<Workout> WorkoutSubCategories
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
                WorkoutCategories = await _workoutService.GetWorkoutCategoriesAsync();
                if (WorkoutCategories.Any())
                {
                    SelectedWorkoutCategory = WorkoutCategories.FirstOrDefault();
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

        public DelegateCommand<Workout> OnSelectedWorkoutCategoryCommand => new DelegateCommand<Workout>(OnSelectedWorkoutCategory);


        private async void OnSelectedWorkoutCategory(Workout value)
        {
            WorkoutSubCategories = await _workoutService.GetWorkoutSubCategoriesAsync(value.Id);
        }

        #endregion

        #endregion
    }
}