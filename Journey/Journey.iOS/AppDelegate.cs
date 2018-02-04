﻿using System;
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

            App.Init(this);

            LoadApplication(new App(new IosInitializer()));

            return base.FinishedLaunching(app, options);
        }

        public async Task<MobileServiceUser> Authenticate()
        {
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                if (user == null)
                {
                    UIWindow window = UIApplication.SharedApplication.KeyWindow;
                    UIViewController viewController = window.RootViewController;
                    if (viewController != null)
                    {
                        while (viewController.PresentedViewController != null)
                            viewController = viewController.PresentedViewController;

                        user = await App.Client.LoginAsync(viewController, MobileServiceAuthenticationProvider.Facebook, Constant.AppName);
                    }
                }
            }
            catch (Exception ex)
            {
            }
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