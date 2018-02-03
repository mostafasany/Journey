using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Microsoft.WindowsAzure.MobileServices;
using Prism;
using Prism.Ioc;
using Journey.Constants;
using Journey.Services;
namespace Journey.UWP
{
    public sealed partial class MainPage : IAuthenticate
    {
        private readonly MobileServiceClient client;
        private MobileServiceUser user;

        public MainPage()
        {
            InitializeComponent();
            client = new MobileServiceClient(Constant.ApplicationUrl);
            Journey.App.Init(this);
            LoadApplication(new Journey.App(new UwpInitializer()));
        }

        public async Task<MobileServiceUser> Authenticate()
        {
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                if (user == null)
                {
                    user = await client.LoginAsync(MobileServiceAuthenticationProvider.Facebook,
                        Constant.ApplicationUrl);
                    if (user != null)
                    {
                        message = string.Format("You are now signed-in as {0}.", user.UserId);
                    }
                }
            }
            catch (Exception ex)
            {
                message = string.Format("Authentication Failed: {0}", ex.Message);
            }

            // Display the success or failure message.
            await new MessageDialog(message, "Sign-in result").ShowAsync();

            return user;
        }
    }

    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}