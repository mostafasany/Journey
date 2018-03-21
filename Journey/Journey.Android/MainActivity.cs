using System;
using System.Threading.Tasks;
using Android.App;
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
    [Activity(Label = "Journey", Icon = "@drawable/icon", Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity, IAzureAuthenticateService
    {
        private MobileServiceUser _user;

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

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            CachedImageRenderer.Init(false);
            VideoViewRenderer.Init();
            App.Init(this);
            CrossCurrentActivity.Current.Activity = this;
            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}