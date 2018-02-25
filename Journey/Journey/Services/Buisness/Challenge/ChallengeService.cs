using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Account.Data;
using Journey.Services.Buisness.Challenge.Data;
using Journey.Services.Buisness.Friend;

namespace Journey.Services.Buisness.Challenge
{
    public class ChallengeService : IChallengeService
    {
        private readonly IAccountService _accountService;
        private readonly IFriendService _friendService;
        private readonly IAccountDataService accountDataService;
        private readonly IChallengeDataService challengeDataService;
        private Models.Challenge.Challenge Challenge;

        public ChallengeService(IChallengeDataService _challengeDataService,
            IAccountDataService _accountDataService,
            IFriendService friendService,
            IAccountService accountService)
        {
            challengeDataService = _challengeDataService;
            accountDataService = _accountDataService;
            _friendService = friendService;
            _accountService = accountService;
        }

        public async Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId)
        {
            try
            {
                var challengeDTO = await challengeDataService.GetChallengeAsync(challengeId);
                return challengeDTO;
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
                var challengeDTO = await challengeDataService.AddChallengeAsync(challenge);
                return challengeDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> GetAccountChallengeAsync()
        {
            try
            {
                //_accountService.LoggedInAccount.ChallengeId = "";
                //await accountDataService.AddUpdateAccountAsync(_accountService.LoggedInAccount, false);
                if (Challenge != null)
                    return Challenge;
                Challenge = await challengeDataService.GetAccountChallengeAsync();
                if (Challenge == null)
                    return null;
                var challenge1 = Challenge.ChallengeAccounts.FirstOrDefault();
                var challenge2 = Challenge.ChallengeAccounts.LastOrDefault();
                var friendId = Challenge.ChallengeAccounts.FirstOrDefault(a => a.Id != _accountService.LoggedInAccount.Id)?.Id;
                var friend = await _friendService.GetFriendAsync(friendId);
                Models.Account.Account account1 = null;
                Models.Account.Account account2 = null;
                if (challenge1.Id == _accountService.LoggedInAccount.Id)
                {
                    account1 = _accountService.LoggedInAccount;
                    account2 = friend;
                }
                else
                {
                    account1 = friend;
                    account2 = _accountService.LoggedInAccount;
                }
                Challenge.ChallengeAccounts = new ObservableCollection<ChallengeAccount>();
                Challenge.ChallengeAccounts.Add(new ChallengeAccount(account1) { NumberExercise = challenge1.NumberExercise });
                Challenge.ChallengeAccounts.Add(new ChallengeAccount(account2) { NumberExercise = challenge2.NumberExercise });
                await EndChallengeAsync(Challenge);
                return Challenge;
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
                var challengeDTO = await challengeDataService.UpdateChallengeAsync(challenge);
                var account1 = challenge.ChallengeAccounts.FirstOrDefault();
                var account2 = challenge.ChallengeAccounts.LastOrDefault();
                account1.ChallengeId = "";
                account2.ChallengeId = "";
                await accountDataService.AddUpdateAccountAsync(account1, false);
                await accountDataService.AddUpdateAccountAsync(account2, false);
                return challengeDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}