﻿using System;
using System.Collections.ObjectModel;
using Abstractions.Services;
using Journey.Models;
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class NotificationsPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IDeepLinkService _deepLinking;
        private readonly INotificationService _postCommentService;

        public NotificationsPageViewModel(IUnityContainer container,
                                          INotificationService postCommentService, IDeepLinkService deepLinking) :
            base(container)
        {
            _postCommentService = postCommentService;
            _deepLinking = deepLinking;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                Intialize();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        #endregion

        #region Properties

        private ObservableCollection<Notifications> _notifications;

        public ObservableCollection<Notifications> Notifications
        {
            get => _notifications;
            set
            {
                _notifications = value;
                RaisePropertyChanged();
            }
        }

        Notifications selectedNotification;
        public Notifications SelectedNotification
        {
            get => selectedNotification;
            set
            {
                SetProperty(ref selectedNotification, value);

                if (value != null)
                    OnSelectedNotificationCommand.Execute(value);
            }
        }

        private bool isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => isPullRefreshLoading;
            set => SetProperty(ref isPullRefreshLoading, value);
        }

        #endregion

        #region Methods

        public override async void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                base.Intialize(sync);

                Notifications = new ObservableCollection<Notifications>();

                var postDTo = await _postCommentService.GetNotificationsAsync();
                if (postDTo != null)
                    Notifications = new ObservableCollection<Notifications>(postDTo);
            }
            catch (Exception e)
            {
                ExceptionService.HandleAndShowDialog(e);
            }
            finally
            {
                HideProgress();
            }
        }

        protected override void Cleanup()
        {
            try
            {
                //Here Cleanup any resources
                base.Cleanup();
            }
            catch (Exception e)
            {
                ExceptionService.HandleAndShowDialog(e);
            }
        }

        #endregion

        #region Commands

        #region OnCloseCommand

        public DelegateCommand OnCloseCommand => new DelegateCommand(OnClose);

        private async void OnClose()
        {
            NavigationService.GoBack();
        }

        #endregion

        #region OnPullRefreshRequestCommand

        public DelegateCommand OnPullRefreshRequestCommand => new DelegateCommand(OnPullRefreshRequest);

        private async void OnPullRefreshRequest()
        {
            try
            {
                IsPullRefreshLoading = true;

                Intialize();
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
            finally
            {
                IsPullRefreshLoading = false;
                HideProgress();
            }
        }

        #endregion


        #region OnSelectedFriendCommand


        public DelegateCommand<Notifications> OnSelectedNotificationCommand => new DelegateCommand<Notifications>(OnSelectedNotification);

        private void OnSelectedNotification(Notifications notification)
        {
            try
            {
                _deepLinking.ParseDeepLinkingAndExecute(notification?.DeepLink);
            }
            catch (System.Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }


        #endregion

        #endregion
    }


      
}