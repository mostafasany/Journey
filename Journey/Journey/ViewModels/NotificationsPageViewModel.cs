using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Abstractions.Services;
using Journey.Models;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class NotificationsPageViewModel : MainNavigationViewModel, INavigationAware
    {
        private readonly IDeepLinkService _deepLinking;
        private readonly INotificationService _postCommentService;

        public NotificationsPageViewModel(IUnityContainer container, IAccountService accountService,
            INotificationService postCommentService, IDeepLinkService deepLinking) :
        base(container, accountService)
        {
            _postCommentService = postCommentService;
            _deepLinking = deepLinking;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
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

        private Notifications _selectedNotification;

        public Notifications SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                SetProperty(ref _selectedNotification, value);

                if (value != null)
                    OnSelectedNotificationCommand.Execute(value);
            }
        }

        private bool _isPullRefreshLoading;

        public bool IsPullRefreshLoading
        {
            get => _isPullRefreshLoading;
            set => SetProperty(ref _isPullRefreshLoading, value);
        }

        private bool _noNofications;

        public bool NoNofications
        {
            get => _noNofications;
            set => SetProperty(ref _noNofications, value);
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
                List<Notifications> postDTo = await _postCommentService.GetNotificationsAsync();
                if (postDTo != null)
                    Notifications = new ObservableCollection<Notifications>(postDTo);

                NoNofications = Notifications == null || Notifications.Count == 0;
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

        private void OnClose()
        {
            NavigationService.GoBack();
        }

        #endregion

        #region OnPullRefreshRequestCommand

        public DelegateCommand OnPullRefreshRequestCommand => new DelegateCommand(OnPullRefreshRequest);

        private void OnPullRefreshRequest()
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

        public DelegateCommand<Notifications> OnSelectedNotificationCommand => new DelegateCommand<Notifications>(
            OnSelectedNotification);

        private void OnSelectedNotification(Notifications notification)
        {
            try
            {
                _deepLinking.ParseDeepLinkingAndExecute(notification?.DeepLink);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion

        #endregion
    }
}