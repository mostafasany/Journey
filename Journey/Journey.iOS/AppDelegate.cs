using System.Threading.Tasks;
using FFImageLoading.Forms.Touch;
using FFImageLoading.Transformations;
using Foundation;
using Journey.Constants;
using Journey.iOS.Renderers;
using Journey.Services.Azure;
using Microsoft.WindowsAzure.MobileServices;
using Prism;
using Prism.Ioc;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Journey.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate, IAzureAuthenticateService
    {
        private MobileServiceUser _user;

        public async Task<MobileServiceUser> AuthenticateAsync()
        {
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                if (_user == null)
                {
                    UIWindow window = UIApplication.SharedApplication.KeyWindow;
                    UIViewController viewController = window.RootViewController;
                    if (viewController != null)
                    {
                        while (viewController.PresentedViewController != null)
                            viewController = viewController.PresentedViewController;

                        _user = await App.Client.LoginAsync(viewController, MobileServiceAuthenticationProvider.Facebook,
                            Constant.AppName);
                    }
                }
            }
            catch
            {
            }

            return _user;
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                foreach (var cookie in NSHttpCookieStorage.SharedStorage.Cookies)
                {
                    NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);
                }
                await App.Client.LogoutAsync();

                _user = null;
            }
            catch
            {
                return false;
            }
            return true;
        }

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

            App.Init(this);
            CachedImageRenderer.Init();
            VideoViewRenderer.Init();
            //https://github.com/luberda-molinet/FFImageLoading/issues/462
            var ignore = new CircleTransformation();
            LoadApplication(new App(new IosInitializer()));

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options) => App.Client.ResumeWithURL(url);
    }


    public class IosInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}