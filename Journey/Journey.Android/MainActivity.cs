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
using Journey.Services;
namespace Journey.Droid
{
    [Activity(Label = "Journey", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity, IAuthenticate
    {
        private MobileServiceUser user;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Journey.App.Init((IAuthenticate)this);
            LoadApplication(new App(new AndroidInitializer()));
        }

        public async Task<MobileServiceUser> Authenticate()
        {
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await Journey.App.Client.LoginAsync(this,
                    MobileServiceAuthenticationProvider.Facebook, Constant.AppName);

            }
            catch (Exception ex)
            {
            }

            return user;
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}