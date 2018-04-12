using System;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Droid;
using Journey.Constants;
using Journey.Droid.Renderers;
using Journey.Services.Azure;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Journey.Droid
{
    [Activity(Label = "Journey", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity, IAzureAuthenticateService
    {
        private MobileServiceUser _user;
        private static int REQUEST_OAUTH = 1;
        private static String AUTH_PENDING = "auth_state_pending";
        private bool authInProgress = false;
        Android.Gms.Common.Apis.GoogleApiClient mClient;
        public async Task<MobileServiceUser> Authenticate()
        {
            try
            {
                if (_user == null)
                    _user = await App.Client.LoginAsync(this,
                        MobileServiceAuthenticationProvider.Facebook, Constant.AppName);
            }
            catch (Exception)
            {
                // ignored
            }

            return _user;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == REQUEST_OAUTH)
            {
                authInProgress = false;
                if (resultCode == Result.Ok)
                {
                    if (!mClient.IsConnecting && !mClient.IsConnected)
                    {
                        mClient.Connect();
                    }
                }
                else if (resultCode == Result.Canceled)
                {
                    //Log.e("GoogleFit", "RESULT_CANCELED");
                }
            }
            else
            {
                // Log.e("GoogleFit", "requestCode NOT request_oauth");
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            //https://blog.xamarin.com/3-big-things-explore-xamarin-forms-2-5-0-pre-release/
            Forms.SetFlags("FastRenderers_Experimental");
            Forms.Init(this, bundle);

            CachedImageRenderer.Init(false);
            VideoViewRenderer.Init();
            App.Init(this);
            CrossCurrentActivity.Current.Activity = this;


            if (bundle != null)
            {
                authInProgress = bundle.GetBoolean(AUTH_PENDING);
            }
            var clientConnectionCallback = new Services.Fitness.ClientConnectionCallback();
            clientConnectionCallback.OnConnectedImpl =async () => await Services.Fitness.FitnessService.FindFitnessDataSources(mClient);
            mClient = new Android.Gms.Common.Apis.GoogleApiClient.Builder(this)
                     .AddApi(Android.Gms.Fitness.FitnessClass.SENSORS_API)
                     .AddScope(new Android.Gms.Common.Apis.Scope(Android.Gms.Common.Scopes.FitnessActivityReadWrite))
                     .AddConnectionCallbacks(clientConnectionCallback)
                     .AddOnConnectionFailedListener(FailedToConnect)
                    .Build();

            LoadApplication(new App(new AndroidInitializer()));
        }

        void FailedToConnect(Android.Gms.Common.ConnectionResult result)
        {
            if (!authInProgress)
            {
                try
                {
                    authInProgress = true;
                    result.StartResolutionForResult(this, REQUEST_OAUTH);
                }
                catch (IntentSender.SendIntentException e)
                {

                }
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            mClient.Connect();
        }
     
        protected override void OnStop()
        {
            base.OnStop();
            if (mClient.IsConnected)
            {
                mClient.Disconnect();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutBoolean(AUTH_PENDING, authInProgress);
        }

    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}