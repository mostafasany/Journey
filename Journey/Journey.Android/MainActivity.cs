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
using Journey.Constants;
using Journey.Services.Azure;

namespace Journey.Droid
{
    [Activity(Label = "Journey", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity, IAzureAuthenticate
    {
        private MobileServiceUser _user;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Journey.App.Init((IAzureAuthenticate) this);
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
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}