using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Account.Data;
using Journey.Services.Buisness.Challenge.Data;
using Journey.Services.Buisness.Notification;

namespace Journey.Services.Buisness.Challenge
{
    public class ChallengeService : IChallengeService
    {
        private readonly IAccountDataService _accountDataService;
        private readonly IAccountService _accountService;
        private readonly IChallengeDataService _challengeDataService;
        private Models.Challenge.Challenge _cachedChallenge;
        public ChallengeService(IChallengeDataService challengeDataService,
            IAccountDataService accountDataService,
            IAccountService accountService)
        {
            _challengeDataService = challengeDataService;
            _accountDataService = accountDataService;
            _accountService = accountService;
        }

        public async Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId)
        {
            try
            {
                if (string.IsNullOrEmpty(challengeId))
                    return null;
                if (_cachedChallenge != null)
                    return _cachedChallenge;

                _cachedChallenge = await _challengeDataService.GetChallengeAsync(challengeId);

                return _cachedChallenge;
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
                Models.Challenge.Challenge challengeDto = await _challengeDataService.AddChallengeAsync(challenge);
                if (challengeDto != null)
                {
                    ChallengeAccount toChallnegeAccount = challengeDto.ChallengeAccounts.LastOrDefault();

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

        public async Task<bool> HasChallengeAsync(string friend)
        {
            try
            {
                Models.Challenge.Challenge challenge = await _challengeDataService.HasChallengeAsync(friend);
                if (challenge == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> ApproveChallengeAsync(string challengeId)
        {
            try
            {
                var challenge = await _challengeDataService.GetChallengeAsync(challengeId);
                if (challenge == null)
                    return null;
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
        public async Task<List<Models.Challenge.Challenge>> GetChallengeRequestsAsync()
        {
            try
            {
                var challenges = await _challengeDataService.GetChallengeRequestsAsync();
                return challenges;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}