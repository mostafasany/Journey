using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Journey.Constants;
using Journey.Models.Account;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
                            Label = "Camera",
                            Invoked = async() =>
                                {
                                   var media=await CrossMedia.Current.TakePhotoAsync(
                                        new StoreCameraMediaOptions {AllowCropping = true});
                                    Stream s=media.GetStream();
                                 var array=  ReadFully(s);
                                    LoggedInAccount.Image = new Media()
                                    {
                                        Path = media.Path,
                                        SourceArray = array,
                                        //Ext = media.Ext,
                                    };
                                }
                        },
                        new DialogCommand
                        {
                            Label = "Gallery",
                            Invoked =async () =>
                            {
                                var media=await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());
                                Stream s=media.GetStream();
                                var array=  ReadFully(s);
                                LoggedInAccount.Image = new Media()
                                {
                                    Path = media.Path,
                                    SourceArray = array,
                                    //Ext = media.Ext,
                                };
                            }
                        },
                        new DialogCommand
                        {
                            Label = "Cancel"
                        }
                    };

                await DialogService.ShowMessageAsync("Take Photo/", "Upload your profile picture", commands);

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

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
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
                    await DialogService.ShowMessageAsync(AppResource.UpdateProfile_FirstNameRequired, AppResource.Error);
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
                    if (string.IsNullOrEmpty(ex))
                        ex = Constant.DefaultImageExt;
                    var fileName = string.Format("{0}.{1}", id, ex);
                    var path = await _blobService.UploadAsync(LoggedInAccount.Image.SourceArray, fileName);
                    LoggedInAccount.Image.Path = path;
                }
                LoggedInAccount = await _accountService.SaveAccountAsync(LoggedInAccount);
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