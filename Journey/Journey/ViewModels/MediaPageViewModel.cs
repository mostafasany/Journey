using System;
using System.Collections.Generic;
using Journey.Models;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.PostComment;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class MediaPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IPostCommentService _postCommentService;

        public MediaPageViewModel(IUnityContainer container,
            IPostCommentService postCommentService, IAccountService accountService) :
            base(container)
        {
            _postCommentService = postCommentService;
            _accountService = accountService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                MediaList = parameters.GetValue<IEnumerable<Media>>("Media") ?? null;
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

        private IEnumerable<Media> mediaList;

        public IEnumerable<Media> MediaList
        {
            get => mediaList;
            set
            {
                mediaList = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        public override void Intialize()
        {
            try
            {
                ShowProgress();
                base.Intialize();
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
            //NavigationService.PopModalAsync();
        }

        #endregion

        #endregion
    }
}