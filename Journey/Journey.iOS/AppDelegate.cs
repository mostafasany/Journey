using System;
using System.Threading.Tasks;
using Foundation;
using Prism;
using Prism.Ioc;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Journey.Constants;
using Journey.Services;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate, IAuthenticate
    {
        private MobileServiceUser user;
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            LoadApplication(new App(new IosInitializer()));
           
            Journey.App.Init(this);
            return base.FinishedLaunching(app, options);
        }

        public async Task<MobileServiceUser> Authenticate()
        {
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                if (user == null)
                {
                    user = await Journey.App.Client
                        .LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController,
                                    MobileServiceAuthenticationProvider.Facebook, Constant.AppName);
                    if (user != null)
                    {
                        message = $"You are now signed-in as {user.UserId}.";
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            UIAlertView avAlert = new UIAlertView("Sign-in result", message, null, "OK", null);
            avAlert.Show();

            return user;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return Journey.App.Client.ResumeWithURL(url);
        }
    }


    public class IosInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}