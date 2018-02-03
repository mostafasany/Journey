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
        private MobileServiceClient client;

        private MobileServiceUser user;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            client = new MobileServiceClient(Constant.ApplicationUrl);

            Journey.App.Init((IAuthenticate) this);
            LoadApplication(new App(new AndroidInitializer()));
        }

        public async Task<MobileServiceUser> Authenticate()
        {
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await client.LoginAsync(this,
                    MobileServiceAuthenticationProvider.Facebook, Constant.ApplicationUrl);
                if (user != null)
                {
                    message = $"you are now signed-in as {user.UserId}.";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

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