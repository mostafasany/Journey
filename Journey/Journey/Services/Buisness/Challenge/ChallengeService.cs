using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Challenge;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Account.Data;
using Journey.Services.Buisness.Challenge.Data;
using Journey.Services.Buisness.Friend;
using Journey.Services.Buisness.Notification;

namespace Journey.Services.Buisness.Challenge
{
    public class ChallengeService : IChallengeService
    {
        private readonly IAccountService _accountService;
        private readonly IFriendService _friendService;
        private readonly INotificationService _notificationService;
        private readonly IAccountDataService _accountDataService;
        private readonly IChallengeDataService _challengeDataService;
       // private Models.Challenge.Challenge _challenge;

        public ChallengeService(IChallengeDataService challengeDataService,
            IAccountDataService accountDataService,
            IFriendService friendService,
            IAccountService accountService,
            INotificationService notificationService)
        {
            _challengeDataService = challengeDataService;
            _accountDataService = accountDataService;
            _friendService = friendService;
            _accountService = accountService;
            _notificationService = notificationService;
        }

        public async Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId)
        {
            try
            {
                //if (_challenge != null)
                   // return _challenge;
               var _challenge = await _challengeDataService.GetChallengeAsync(challengeId);
                return _challenge;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> AddChallengeAsync(Models.Challenge.Challenge challenge)
        {
            try
            {
                //var notification = await _notificationService.AddNotificationAsync(
                //   new Models.Notifications
                //   {
                //Account = _accountService.LoggedInAccount,
                //    Message = string.Format(AppResource.Notification_ChallengeRequestMessage, _accountService.LoggedInAccount.Name),
                //    Title = AppResource.Notification_ChallengeRequestTitle,
                //    DeepLink = string.Format("http://www.journey.challengeRequest?id={0}", "1"),
                //});

                var challengeDto = await _challengeDataService.AddChallengeAsync(challenge);
                if (challengeDto != null)
                {
                    var toChallnegeAccount = challengeDto.ChallengeAccounts.LastOrDefault();
                    var notification = await _notificationService.AddNotificationAsync(
                         new Models.Notifications
                         {
                             Account = toChallnegeAccount,
                             Message = string.Format(AppResource.Notification_ChallengeRequestMessage, _accountService.LoggedInAccount.Name),
                             Title = AppResource.Notification_ChallengeRequestTitle,
                        DeepLink = string.Format("http://www.journey.challengeRequest?id={0}", challengeDto.Id),
                         });
                    if (notification != null)
                        return challengeDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> CheckAccountHasChallengeAsync()
        {
            try
            {
                //_accountService.LoggedInAccount.ChallengeId = "";
                //await accountDataService.AddUpdateAccountAsync(_accountService.LoggedInAccount, false);
               
               var _challenge = await _challengeDataService.CheckAccountHasChallengeAsync();
                if (_challenge == null)
                    return false;
                return true;
                //var challenge1 = _challenge.ChallengeAccounts.FirstOrDefault();
                //var challenge2 = _challenge.ChallengeAccounts.LastOrDefault();
                //var friendId = _challenge.ChallengeAccounts.FirstOrDefault(a => a.Id != _accountService.LoggedInAccount.Id)?.Id;
                //var friend = await _friendService.GetFriendAsync(friendId);
                //Models.Account.Account account1;
                //Models.Account.Account account2;
                //if (challenge1.Id == _accountService.LoggedInAccount.Id)
                //{
                //    account1 = _accountService.LoggedInAccount;
                //    account2 = friend;
                //}
                //else
                //{
                //    account1 = friend;
                //    account2 = _accountService.LoggedInAccount;
                //}
                //_challenge.ChallengeAccounts = new ObservableCollection<ChallengeAccount>();
                //_challenge.ChallengeAccounts.Add(new ChallengeAccount(account1) { NumberExercise = challenge1.NumberExercise });
                //_challenge.ChallengeAccounts.Add(new ChallengeAccount(account2) { NumberExercise = challenge2.NumberExercise });

               // return _challenge;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> ApproveChallengeAsync(Models.Challenge.Challenge challenge)
        {
            try
            {
                challenge.IsActive = true;
                var challengeDto = await _challengeDataService.UpdateChallengeAsync(challenge);
                var account1 = challenge.ChallengeAccounts.FirstOrDefault();
                var account2 = challenge.ChallengeAccounts.LastOrDefault();
                account1.ChallengeId = challenge.Id;
                account2.ChallengeId = challenge.Id;
                await _accountDataService.AddUpdateAccountAsync(account1, false);
                await _accountDataService.AddUpdateAccountAsync(account2, false);
                return challengeDto;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> EndChallengeAsync(Models.Challenge.Challenge challenge)
        {
            try
            {
                challenge.IsActive = false;
                var challengeDto = await _challengeDataService.UpdateChallengeAsync(challenge);
                var account1 = challenge.ChallengeAccounts.FirstOrDefault();
                var account2 = challenge.ChallengeAccounts.LastOrDefault();
                account1.ChallengeId = "";
                account2.ChallengeId = "";
                await _accountDataService.AddUpdateAccountAsync(account1, false);
                await _accountDataService.AddUpdateAccountAsync(account2, false);
                return challengeDto;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> UpdateExerciseNumberAsync(string challengeId)
        {
            try
            {
                var challenge = await GetChallengeAsync(challengeId);
                var account=challenge.ChallengeAccounts.FirstOrDefault(a=>a.Id==_accountService.LoggedInAccount.Id);
                account.NumberExercise++;
                var challengeDto = await _challengeDataService.UpdateChallengeAsync(challenge);
                return challengeDto;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}