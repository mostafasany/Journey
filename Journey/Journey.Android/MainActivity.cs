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
        public static Android.Gms.Common.Apis.GoogleApiClient mClient;
        IHealthService _healthService => DependencyService.Get<IHealthService>();
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
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == REQUEST_OAUTH)
            {
                _healthService.AuthInProgress = false;
                if (resultCode == Result.Ok)
                {
                    if (!mClient.IsConnecting && !mClient.IsConnected)
                    {
                        mClient.Connect();
                    }
                }
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
                if (_healthService != null)
                    _healthService.AuthInProgress = bundle.GetBoolean(AUTH_PENDING);
            }

            LoadApplication(new App(new AndroidInitializer()));
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (mClient == null)
                return;

            mClient.Connect();
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (mClient == null)
                return;

            if (mClient.IsConnected)
            {
                mClient.Disconnect();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            if (_healthService != null)
                outState.PutBoolean(AUTH_PENDING, _healthService.AuthInProgress);
        }

    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}