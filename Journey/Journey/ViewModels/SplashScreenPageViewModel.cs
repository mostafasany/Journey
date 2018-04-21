using System;
using Abstractions.Forms;
using Abstractions.Services;
using Abstractions.Services.Contracts;
using Journey.Services.Azure;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Account.Data;
using Journey.Services.Buisness.Blob;
using Journey.Services.Buisness.Challenge;
using Journey.Services.Buisness.Challenge.Data;
using Journey.Services.Buisness.ChallengeActivity;
using Journey.Services.Buisness.ChallengeActivity.Data;
using Journey.Services.Buisness.DeepLink;
using Journey.Services.Buisness.Friend;
using Journey.Services.Buisness.Friend.Data;
using Journey.Services.Buisness.Goal;
using Journey.Services.Buisness.Goal.Data;
using Journey.Services.Buisness.Measurment;
using Journey.Services.Buisness.Measurment.Data;
using Journey.Services.Buisness.Notification;
using Journey.Services.Buisness.Notification.Data;
using Journey.Services.Buisness.Post;
using Journey.Services.Buisness.Post.Data;
using Journey.Services.Buisness.PostComment;
using Journey.Services.Buisness.PostComment.Data;
using Journey.Services.Buisness.Workout;
using Journey.Services.Buisness.Workout.Data;
using Prism.Navigation;
using Unity;
using Unity.Lifetime;
using INavigationService = Prism.Navigation.INavigationService;

namespace Journey.ViewModels
{
    public class SplashScreenPageViewModel : BaseViewModel, INavigationAware
    {
        public SplashScreenPageViewModel(IUnityContainer container) :
            base(container)
        {
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Intialize();
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            RegitserAppServices(Container);
            RegitserBuisnessServices(Container);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                var settingsService = Container.Resolve<ISettingsService>();
                var navigationService = Container.Resolve<INavigationService>();
                if (settingsService != null)
                {
                    var azureService = Container.Resolve<IAzureService>();
                    var accountService = Container.Resolve<IAccountService>();
                    var facebookService = Container.Resolve<IFacebookService>();
                    facebookService.FacebookToken = await settingsService.Get(facebookService.FacebookTokenKey);
                    string userToken = await settingsService.Get(accountService.AccountTokenKey);
                    string userId = await settingsService.Get(accountService.AccountIdKey);
                    accountService.Token = userToken;

                    azureService.CreateOrGetAzureClient(userId, userToken);

                    await navigationService.NavigateAsync(string.IsNullOrEmpty(userId) ? "LoginPage" : "HomePage");
                }
                else
                {
                    await navigationService.NavigateAsync("LoginPage");
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

        public static void ConfigureDialogService(IUnityContainer container)
        {
            container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            if (!(container.Resolve<IDialogService>() is DialogService dialogService)) return;
            dialogService.ErrorMessageTitle = "Error Occured";
            dialogService.ErrorMessageBody = "Please try again later";
            dialogService.NoInternetMessageBody = "No internet connection available,Please reconnect and try again later";
            dialogService.NoInternetMessageTitle = "No internet";
        }

        public static void RegitserAppServices(IUnityContainer container)
        {
            container.RegisterInstance(typeof(IUnityContainer), container);
            container.RegisterType<IExceptionService, ExceptionService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IHttpService, HttpService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILoggerService, LoggerService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMediaService<Media>, MediaService>(new ContainerControlledLifetimeManager());
            container.RegisterType<Abstractions.Services.Contracts.INavigationService, NavigationService>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<INavigationService, PageNavigationService>(
                new ContainerControlledLifetimeManager());

            container.RegisterType<ISerializerService, SerializerService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IShareService, ShareService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILocationService, LocationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IBlobService, BlobService>(new ContainerControlledLifetimeManager());

            ConfigureDialogService(container);
        }

        public static void RegitserBuisnessServices(IUnityContainer container)
        {
            container.RegisterType<IAzureService, AzureService>(new ContainerControlledLifetimeManager());

            container.RegisterType<IPostService, PostService>(new ContainerControlledLifetimeManager());

            //if (Device.RuntimePlatform == Device.WinPhone)
            //{
            //    container.RegisterType<IPostDataService, PostDataMockService>(new ContainerControlledLifetimeManager());
            //    container.RegisterType<IAccountDataService, AccountDataMockService>(new ContainerControlledLifetimeManager());
            //}
            //else
            {
                container.RegisterType<IPostDataService, PostDataService>(new ContainerControlledLifetimeManager());
                container.RegisterType<IAccountDataService, AccountDataService>(new ContainerControlledLifetimeManager());
            }

            container.RegisterType<IPostCommentService, PostCommentService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPostCommentDataService, PostCommentDataService>(
                new ContainerControlledLifetimeManager());

            container.RegisterType<IFacebookService, FacebookService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFriendService, FriendService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFriendDataService, FriendDataService>(new ContainerControlledLifetimeManager());

            container.RegisterType<INotificationDataService, NotificationDataService>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<INotificationService, NotificationService>(new ContainerControlledLifetimeManager());

            container.RegisterType<IChallengeService, ChallengeService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IChallengeDataService, ChallengeDataService>(
                new ContainerControlledLifetimeManager());

            container.RegisterType<IChallengeActivityService, ChallengeActivityService>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IChallengeActivityDataService, ChallengeActivityDataService>(
                new ContainerControlledLifetimeManager());

            container.RegisterType<IDeepLinkService, DeepLinkService>(new ContainerControlledLifetimeManager());

            container.RegisterType<IAccountGoalService, AccountGoalService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAccountGoalDataService, AccountGoalDataService>(
                new ContainerControlledLifetimeManager());

            container.RegisterType<IAccountMeasurmentService, AccountMeasurmentService>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IAccountMeasurmentDataService, AccountMeasurmentDataService>(
                new ContainerControlledLifetimeManager());

            container.RegisterType<IWorkoutService, WorkoutService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IWorkoutDataService, WorkoutDataService>(new ContainerControlledLifetimeManager());
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

        #endregion
    }
}