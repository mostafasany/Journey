using System;
using System.Collections.Generic;
using System.Windows.Input;
using Abstractions.Forms;
using Abstractions.Services.Contracts;
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
                var loggedInAccount = parameters.GetValue<Account>("Account") ?? new Account();
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

        private string firstName;

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string lastName;

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }


        private Media image;

        public Media Image
        {
            get => image;
            set => SetProperty(ref image, value);
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
                            catch (NotSupportedException ex)
                            {
                                await DialogService.ShowMessageAsync(AppResource.Camera_NotSupported,AppResource.Error);
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
                    var path = await _blobService.UploadAsync(Image.SourceArray, Image.Name);
                    Image.Path = path;
                }
                var account = _accountService.LoggedInAccount;
                account.FirstName = FirstName;
                account.LastName = LastName;
                account.Image = Image;
                await _accountService.SaveAccountAsync(account, false);
                if (ComeFromProfile)
                    NavigationService.GoBack();
                else
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

        #region OnBackCommand

        public DelegateCommand OnBackCommand => new DelegateCommand(OnBack);


        private async void OnBack()
        {
            try
            {
                NavigationService.GoBack();
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

        #endregion

        #endregion
    }
}