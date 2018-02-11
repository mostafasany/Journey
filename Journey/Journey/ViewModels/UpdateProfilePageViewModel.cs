using System;
using System.Collections.Generic;
using System.Windows.Input;
using Abstractions.Services.Contracts;
using Journey.Constants;
using Journey.Models;
using Journey.Models.Account;
using Journey.Resources;
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
        private readonly IMediaService<Media> _mediaService;

        public UpdateProfilePageViewModel(IUnityContainer container, IAccountService accountService,
            IBlobService blobService, IMediaService<Media> mediaService) :
            base(container)
        {
            _accountService = accountService;
            _blobService = blobService;
            _mediaService = mediaService;
        }

        #region Events

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                ShowProgress();
                LoggedInAccount = parameters.GetValue<Account>("Account") ?? new Account();
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
                var commands =
                    new List<DialogCommand>
                    {
                        new DialogCommand
                        {
                            Label = AppResource.Camera,
                            Invoked = async () => { LoggedInAccount.Image = await _mediaService.TakePhotoAsync(); }
                        },
                        new DialogCommand
                        {
                            Label = AppResource.Gallery,
                            Invoked = async () => { LoggedInAccount.Image = await _mediaService.PickPhotoAsync(); }
                        },
                        new DialogCommand
                        {
                            Label = AppResource.Cancel
                        }
                    };

                await DialogService.ShowMessageAsync(AppResource.UploadPhoto_Message, AppResource.UploadPhoto_Title,
                    commands);
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
                    await DialogService.ShowMessageAsync(AppResource.UpdateProfile_FirstNameRequired,
                        AppResource.Error);
                    return;
                }
                else if (string.IsNullOrEmpty(LoggedInAccount?.Image?.Path))
                {
                    await DialogService.ShowMessageAsync(AppResource.UpdateProfile_ImageRequired, AppResource.Error);
                    return;
                }

                ShowProgress();
                if (LoggedInAccount.Image?.SourceArray != null)
                {
                    var id = Guid.NewGuid();
                    var ex = LoggedInAccount.Image.Ext;
                    var fileName = string.Format("{0}{1}", id, ex);
                    var path = await _blobService.UploadAsync(LoggedInAccount.Image.SourceArray, fileName);
                    LoggedInAccount.Image.Path = path;
                }
                LoggedInAccount = await _accountService.SaveAccountAsync(LoggedInAccount, false);
                await NavigationService.Navigate("HomePage");
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