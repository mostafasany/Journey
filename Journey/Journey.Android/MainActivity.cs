using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Journey.Constants;
using Journey.Services.Azure;
using Permission = Android.Content.PM.Permission;
using FFImageLoading.Forms.Droid;
using Journey.Droid.Renderers;

namespace Journey.Droid
{
    [Activity(Label = "Journey", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity, IAzureAuthenticateService
    {
        private MobileServiceUser _user;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            CachedImageRenderer.Init(false);
            VideoViewRenderer.Init();
            Journey.App.Init((IAzureAuthenticateService) this);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
            LoadApplication(new App(new AndroidInitializer()));
        }

        public async Task<MobileServiceUser> Authenticate()
        {
            try
            {
                if (_user == null)
                {
                    // Sign in with Facebook login using a server-managed flow.
                    _user = await Journey.App.Client.LoginAsync(this,
                        MobileServiceAuthenticationProvider.Facebook, Constant.AppName);
                }
            }
            catch (Exception ex)
            {
            }

            return _user;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}