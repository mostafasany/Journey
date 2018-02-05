using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Prism;
using Prism.Ioc;
using Journey.Constants;
using Journey.Services.Azure;

namespace Journey.UWP
{
    public sealed partial class MainPage : IAzureAuthenticateService
    {
        private MobileServiceUser _user;

        public MainPage()
        {
            InitializeComponent();
            Journey.App.Init(this);
            LoadApplication(new Journey.App(new UwpInitializer()));
        }

        public async Task<MobileServiceUser> Authenticate()
        {
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                if (_user == null)
                    _user = await Journey.App.Client.LoginAsync(MobileServiceAuthenticationProvider.Facebook,
                        Constant.AppName);
            }
            catch (Exception ex)
            {
            }


            return _user;
        }
    }

    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}