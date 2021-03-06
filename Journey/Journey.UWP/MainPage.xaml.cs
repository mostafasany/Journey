﻿using System;
using System.Threading.Tasks;
using FFImageLoading.Forms.WinUWP;
using Journey.Constants;
using Journey.Services.Azure;
using Microsoft.WindowsAzure.MobileServices;
using Prism;
using Prism.Ioc;

namespace Journey.UWP
{
    public sealed partial class MainPage : IAzureAuthenticateService
    {
        private MobileServiceUser _user;

        public MainPage()
        {
            InitializeComponent();
            Journey.App.Init(this);
            CachedImageRenderer.Init();
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
            catch (Exception)
            {
                // ignored
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