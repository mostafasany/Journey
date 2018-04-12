using Journey.Constants;
using Journey.Services.Azure;
using Journey.Views;
using Microsoft.WindowsAzure.MobileServices;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Journey
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        public static IAzureAuthenticateService Authenticator { get; private set; }
        public static MobileServiceClient Client { get; private set; }

        public static void Init(IAzureAuthenticateService authenticator)
        {
            Authenticator = authenticator;
            Client = new MobileServiceClient(Constant.ApplicationUrl);
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("SplashScreenPage");
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SplashScreenPage>();
            containerRegistry.RegisterForNavigation<HomePage>();
            containerRegistry.RegisterForNavigation<LoginPage>();
            containerRegistry.RegisterForNavigation<UpdateProfilePage>();
            containerRegistry.RegisterForNavigation<NewPostPage>();
            containerRegistry.RegisterForNavigation<NewCommentPage>();
            containerRegistry.RegisterForNavigation<ChooseLocationPage>();
            containerRegistry.RegisterForNavigation<MediaPage>();
            containerRegistry.RegisterForNavigation<VideoPage>();
            containerRegistry.RegisterForNavigation<ImagePage>();
            containerRegistry.RegisterForNavigation<ProfileMeasurmentPage>();
            containerRegistry.RegisterForNavigation<ProfileActivityLogPage>();
            containerRegistry.RegisterForNavigation<ChallengeProgressPage>();
            containerRegistry.RegisterForNavigation<UpdateMeasurmentPage>();
            containerRegistry.RegisterForNavigation<ChooseChallengeFriendPage>();
            containerRegistry.RegisterForNavigation<NewChallengePage>();
            containerRegistry.RegisterForNavigation<NotificationsPage>();
            containerRegistry.RegisterForNavigation<ProfileLogWorkoutPage>();
            containerRegistry.RegisterForNavigation<StartNewChallengePage>();
            containerRegistry.RegisterForNavigation<FriendsPage>();
        }
    }
}