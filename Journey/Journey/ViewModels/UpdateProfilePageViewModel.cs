using System;
using System.Windows.Input;
using Abstractions.Services.Contracts;
using Journey.Constants;
using Journey.Models.Account;
using Journey.Services.Buisness.Account;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class UpdateProfilePageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IBlobService _blobService;

        public UpdateProfilePageViewModel(IUnityContainer container, IAccountService accountService,
            IBlobService blobService) :
            base(container)
        {
            _accountService = accountService;
            _blobService = blobService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                ShowProgress();
                LoggedInAccount = parameters.GetValue<Account>("Account");
                // LoggedInAccount = await _accountService.GetAccountAsync();
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
            finally
            {
                HideProgress();
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        #endregion

        #region Properties

        private Account _loggedInAccount;

        public Account LoggedInAccount
        {
            get => _loggedInAccount;
            set => SetProperty(ref _loggedInAccount, value);
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

        #region UploadImageCommand

        private ICommand _uploadImageCommand;


        public ICommand UploadImageCommand => _uploadImageCommand ??
                                              (_uploadImageCommand =
                                                  new DelegateCommand(UploadImage));

        private async void UploadImage()
        {
            try
            {
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

        #endregion

        #region FinishCommand

        private ICommand _finishCommand;


        public ICommand FinishCommand => _finishCommand ??
                                         (_finishCommand =
                                             new DelegateCommand(Finish));

        private async void Finish()
        {
            try
            {
                if (IsProgress())
                    return;

                if (string.IsNullOrEmpty(LoggedInAccount.FirstName))
                {
                    await DialogService.ShowMessageAsync("Error", "You must have First name");
                    return;
                }
                else if (string.IsNullOrEmpty(LoggedInAccount?.Image?.Path))
                {
                    await DialogService.ShowMessageAsync("Error", "You must have Profile picture");
                    return;
                }

                ShowProgress();
                if (LoggedInAccount.Image?.SourceArray != null)
                {
                    var id = Guid.NewGuid();
                    var ex = LoggedInAccount.Image.Ext;
                    if (string.IsNullOrEmpty(ex))
                        ex = Constant.DefaultImageExt;
                    var fileName = string.Format("{0}.{1}", id, ex);
                    var path = await _blobService.UploadAsync(LoggedInAccount.Image.SourceArray, fileName);
                    LoggedInAccount.Image.Path = path;
                }
                LoggedInAccount = await _accountService.SaveAccountAsync(LoggedInAccount);
                await NavigationService.Navigate("Home");
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

        #endregion

        #endregion
    }
}