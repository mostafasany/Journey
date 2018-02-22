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
        private readonly  IChallengeDataService challengeDataService;
        private readonly IAccountDataService accountDataService;
        private readonly IFriendService _friendService;
        private readonly IAccountService _accountService;
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
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public async Task<Models.Challenge.Challenge> SaveCurrentChallengeAsync(Models.Challenge.Challenge challenge)
        {
            try
            {
                var challengeDTO = await challengeDataService.AddChallengeAsync(challenge);
                var account = challenge.ChallengeAccounts.FirstOrDefault();
                account.ChallengeId = challengeDTO.Id;
                await  accountDataService.AddUpdateAccountAsync(account,false);
                return challengeDTO;
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> GetAccountChallengeAsync()
        {
            try
            {
                if (Challenge != null)
                    return Challenge;
                Challenge = await challengeDataService.GetAccountChallengeAsync();
                var friendId = Challenge.ChallengeAccounts.LastOrDefault()?.Id;
                var friend = await _friendService.GetFriendAsync(friendId);
                Challenge.ChallengeAccounts = new System.Collections.ObjectModel.ObservableCollection<ChallengeAccount>();
                Challenge.ChallengeAccounts.Add(new ChallengeAccount(_accountService.LoggedInAccount));
                Challenge.ChallengeAccounts.Add(new ChallengeAccount(friend));
                return Challenge;
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}
