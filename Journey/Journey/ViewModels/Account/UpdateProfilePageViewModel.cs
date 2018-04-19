using System;
using System.Collections.Generic;
using System.Windows.Input;
using Abstractions.Forms;
using Abstractions.Services.Contracts;
using Journey.Models.Account;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Notification;
using Prism.Commands;
using Prism.Navigation;
using Unity;

namespace Journey.ViewModels
{
    public class UpdateProfilePageViewModel : ProfilePageViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IBlobService _blobService;
        private readonly IMediaService<Media> _mediaService;

        public UpdateProfilePageViewModel(IUnityContainer container, IAccountService accountService, INotificationService notificationService,
            IBlobService blobService, IMediaService<Media> mediaService) :
            base(container, accountService, notificationService)
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
                ClearTabSelection();
                FourthTabSelected = "#f1f1f1";

                ShowProgress();
                Account loggedInAccount = parameters.GetValue<Account>("Account") ?? new Account();
                FirstName = loggedInAccount.FirstName;
                LastName = loggedInAccount.LastName;
                Image = loggedInAccount.Image;
                ComeFromProfile = parameters.GetValue<bool>("ComeFromProfile");
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

        private string _firstName;

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }


        private Media _image;

        public new Media Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private bool _comeFromProfile;

        public bool ComeFromProfile
        {
            get => _comeFromProfile;
            set => SetProperty(ref _comeFromProfile, value);
        }

        #endregion

        #region Methods

        public override void Intialize(bool sync = false)
        {
            try
            {
                ShowProgress();
                base.Intialize(sync);
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
                            Invoked = async () =>
                            {
                                try
                                {
                                    Image = await _mediaService.TakePhotoAsync() ?? Image;
                                }
                                catch (NotSupportedException)
                                {
                                    await DialogService.ShowMessageAsync(AppResource.Camera_NotSupported, AppResource.Error);
                                }
                            }
                        },
                        new DialogCommand
                        {
                            Label = AppResource.Gallery,
                            Invoked = async () => { Image = await _mediaService.PickPhotoAsync() ?? Image; }
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

                if (string.IsNullOrEmpty(FirstName))
                {
                    await DialogService.ShowMessageAsync(AppResource.UpdateProfile_FirstNameRequired,
                        AppResource.Error);
                    return;
                }
                else if (string.IsNullOrEmpty(Image?.Path))
                {
                    await DialogService.ShowMessageAsync(AppResource.UpdateProfile_ImageRequired, AppResource.Error);
                    return;
                }

                ShowProgress();
                if (Image?.SourceArray != null)
                {
                    string path = await _blobService.UploadAsync(Image.SourceArray, Image.Name);
                    Image.Path = path;
                }

                Account account = _accountService.LoggedInAccount;
                account.FirstName = FirstName;
                account.LastName = LastName;
                account.Image = Image;
                await _accountService.SaveAccountAsync(account, false);
                if (!ComeFromProfile)
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