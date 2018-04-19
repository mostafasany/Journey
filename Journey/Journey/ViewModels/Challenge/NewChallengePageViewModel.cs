using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Journey.Models.Account;
using Journey.Models.Challenge;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Challenge;
using Prism.Commands;
using Prism.Navigation;
using Unity;
using ChallengeAccount = Journey.Models.Challenge.ChallengeAccount;

namespace Journey.ViewModels
{
    public class NewChallengePageViewModel : BaseViewModel, INavigationAware
    {
        private readonly IAccountService _accountService;
        private readonly IChallengeService _challengeService;

        public NewChallengePageViewModel(IUnityContainer container,
            IChallengeService challengeService, IAccountService accountService) :
            base(container)
        {
            _challengeService = challengeService;
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
                Intialize();
                ShowProgress();
                if (parameters?.GetNavigationMode() == NavigationMode.New)
                {
                    var mode = parameters.GetValue<int>("Mode");
                    IsAddMode = false;
                    IsApproveRequestMode = false;
                    IsEditMode = false;
                    if (mode == 0)
                    {
                        //Add
                        IsAddMode = true;
                        ToChallenge = parameters.GetValue<Account>("ToChallenge");
                        if (string.IsNullOrEmpty(_accountService.LoggedInAccount.ChallengeId))
                        {
                            SelectedChallenge = new Challenge();
                            var challengesAccount = new ObservableCollection<ChallengeAccount>();
                            challengesAccount.Add(new ChallengeAccount(_accountService.LoggedInAccount));
                            challengesAccount.Add(new ChallengeAccount(ToChallenge));
                            SelectedChallenge.ChallengeAccounts = challengesAccount;
                        }
                    }
                    else
                    {
                        var challengeId = parameters.GetValue<string>("Challenge");
                        if (!string.IsNullOrEmpty(challengeId))
                            SelectedChallenge = await _challengeService.GetChallengeAsync(challengeId);

                        if (mode == 1)
                            IsEditMode = true;
                        else if (mode == 2) IsApproveRequestMode = true;
                    }
                }
                else if (parameters?.GetNavigationMode() == NavigationMode.Back)
                {
                    var location = parameters.GetValue<Location>("Location");
                    if (location != null)
                        SelectedChallenge.SelectedLocation = location;
                }
            }
            catch (Exception e)
            {
                ExceptionService.Handle(e);
                await DialogService.ShowGenericErrorMessageAsync(e.Message, AppResource.Error);
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

        private bool _isAddMode;

        public bool IsAddMode
        {
            get => _isAddMode;
            set => SetProperty(ref _isAddMode, value);
        }

        private bool _isEditMode;

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        private bool _isApproveRequestMode;

        public bool IsApproveRequestMode
        {
            get => _isApproveRequestMode;
            set => SetProperty(ref _isApproveRequestMode, value);
        }


        private Account _toChallenge;

        public Account ToChallenge
        {
            get => _toChallenge;
            set => SetProperty(ref _toChallenge, value);
        }

        private Challenge _selectedChallenge;

        public Challenge SelectedChallenge
        {
            get => _selectedChallenge;
            set => SetProperty(ref _selectedChallenge, value);
        }


        //List<Interval> intervalList;
        //public List<Interval> IntervalList
        //{
        //    get => intervalList;
        //    set => SetProperty(ref intervalList, value);
        //}

        //Interval selectedInterval;
        //public Interval SelectedInterval
        //{
        //    get => selectedInterval;
        //    set => SetProperty(ref selectedInterval, value);
        //}

        #endregion

        #region Methods

        public override void Intialize(bool sync = false)
        {
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

        #region OnStartChallengeCommand

        public DelegateCommand OnStartChallengeCommand => new DelegateCommand(OnStartChallenge);

        private async void OnStartChallenge()
        {
            try
            {
                if (IsProgress())
                    return;

                if (SelectedChallenge.StartDate >= SelectedChallenge.EndDate)
                {
                    await DialogService.ShowMessageAsync(AppResource.Error, AppResource.Challenge_DataValidation);
                    return;
                }

                if (SelectedChallenge?.SelectedLocation == null)
                {
                    await DialogService.ShowMessageAsync(AppResource.Post_LocationMust, AppResource.Error);
                    return;
                }

                var startChallengeCommand = new DialogCommand
                {
                    Label = AppResource.Yes,
                    Invoked = () => StartChallenge()
                };

                var cancelCommand = new DialogCommand
                {
                    Label = AppResource.Cancel
                };


                var commands = new List<DialogCommand>
                {
                    startChallengeCommand,
                    cancelCommand
                };
                //SelectedChallenge.Interval = SelectedInterval.IntervalValue;
                await DialogService.ShowMessageAsync("", AppResource.Challenge_Start, commands);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
                await DialogService.ShowGenericErrorMessageAsync(ex.Message, AppResource.Error);
            }
            finally
            {
                HideProgress();
            }
        }

        private async Task StartChallenge()
        {
            try
            {
                if (IsProgress())
                    return;

                ShowProgress();
                bool hasActiveChallange = await _challengeService.CheckAccountHasChallengeAsync();
                if (!hasActiveChallange)
                {
                    if (string.IsNullOrEmpty(SelectedChallenge.SelectedLocation.Id))
                    {
                        await DialogService.ShowMessageAsync(AppResource.Challenge_LocationMust, "");
                        return;
                    }

                    Challenge challenge = await _challengeService.AddChallengeAsync(SelectedChallenge);
                    if (challenge != null)
                    {
                        await DialogService.ShowMessageAsync(AppResource.Challenge_ApproveMessage, "");
                        await NavigationService.Navigate("HomePage", false, "Sync");
                    }
                }
                else
                {
                    await DialogService.ShowMessageAsync(AppResource.Challenge_AlreadyExists, AppResource.Error);
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowGenericErrorMessageAsync(ex.Message, AppResource.Error);
            }
            finally
            {
                HideProgress();
            }
        }

        #endregion

        #region OnApproveRequestCommand

        public DelegateCommand OnApproveRequestCommand => new DelegateCommand(OnApproveRequest);


        private async void OnApproveRequest()
        {
            try
            {
                Challenge challenge = await _challengeService.ApproveChallengeAsync(SelectedChallenge);
                if (challenge != null)
                    await NavigationService.Navigate("HomePage", true, "Sync");
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

        #region OnEditChallengeCommand

        public DelegateCommand OnEditChallengeCommand => new DelegateCommand(OnEditChallenge);


        private async void OnEditChallenge()
        {
            try
            {
                if (IsProgress())
                    return;

                ShowProgress();
                SelectedChallenge = await _challengeService.EditChallengeAsync(SelectedChallenge);
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

        #region OnBackCommand

        public DelegateCommand OnBackCommand => new DelegateCommand(OnBack);


        private void OnBack()
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

        #region OnGetWorkoutLocationCommand

        public DelegateCommand OnGetWorkoutLocationCommand => new DelegateCommand(OnGetWorkoutLocation);

        private async void OnGetWorkoutLocation()
        {
            await NavigationService.Navigate("ChooseLocationPage");
        }

        #endregion

        #endregion
    }
}