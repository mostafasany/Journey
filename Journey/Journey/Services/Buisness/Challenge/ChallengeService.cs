using System;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models;
using Journey.Resources;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Account.Data;
using Journey.Services.Buisness.Challenge.Data;
using Journey.Services.Buisness.Notification;
using ChallengeAccount = Journey.Models.Challenge.ChallengeAccount;

namespace Journey.Services.Buisness.Challenge
{
    public class ChallengeService : IChallengeService
    {
        private readonly IAccountDataService _accountDataService;
        private readonly IAccountService _accountService;
        private readonly IChallengeDataService _challengeDataService;
        private readonly INotificationService _notificationService;

        public ChallengeService(IChallengeDataService challengeDataService,
            IAccountDataService accountDataService,
            IAccountService accountService,
            INotificationService notificationService)
        {
            _challengeDataService = challengeDataService;
            _accountDataService = accountDataService;
            _accountService = accountService;
            _notificationService = notificationService;
        }

        public async Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId)
        {
            try
            {
                if (string.IsNullOrEmpty(challengeId))
                    return null;
                Models.Challenge.Challenge challenge = await _challengeDataService.GetChallengeAsync(challengeId);
                return challenge;
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

                Models.Challenge.Challenge challengeDto = await _challengeDataService.AddChallengeAsync(challenge);
                if (challengeDto != null)
                {
                    ChallengeAccount toChallnegeAccount = challengeDto.ChallengeAccounts.LastOrDefault();
                    Notifications notification = await _notificationService.AddNotificationAsync(
                        new Notifications
                        {
                            Account = toChallnegeAccount,
                            Message = string.Format(AppResource.Notification_ChallengeRequestMessage,
                                _accountService.LoggedInAccount.Name),
                            Title = AppResource.Notification_ChallengeRequestTitle,
                            DeepLink = string.Format("http://www.journey.challengeRequest?id={0}", challengeDto.Id)
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

        public async Task<Models.Challenge.Challenge> EditChallengeAsync(Models.Challenge.Challenge challenge)
        {
            try
            {
                Models.Challenge.Challenge challengeDto = await _challengeDataService.EditChallengeAsync(challenge);

                return challengeDto;
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
                Models.Challenge.Challenge challenge = await _challengeDataService.CheckAccountHasChallengeAsync();
                if (challenge == null)
                    return false;
                return true;
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
                Models.Challenge.Challenge challengeDto = await _challengeDataService.UpdateChallengeAsync(challenge);
                ChallengeAccount account1 = challenge.ChallengeAccounts.FirstOrDefault();
                ChallengeAccount account2 = challenge.ChallengeAccounts.LastOrDefault();
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
                Models.Challenge.Challenge challengeDto = await _challengeDataService.UpdateChallengeAsync(challenge);
                ChallengeAccount account1 = challenge.ChallengeAccounts.FirstOrDefault();
                ChallengeAccount account2 = challenge.ChallengeAccounts.LastOrDefault();
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
    }
}