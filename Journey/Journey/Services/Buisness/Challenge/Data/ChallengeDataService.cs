using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Services.Azure;
using Journey.Services.Buisness.Challenge.Dto;
using Journey.Services.Buisness.Challenge.Translators;
using Journey.Services.Buisness.Friend.Data;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Challenge.Data
{
    public class ChallengeDataService : IChallengeDataService
    {
        IMobileServiceTable<AzureChallenge> azureChallenge;
        private readonly MobileServiceClient _client;
        IFriendDataService friendDataService;
        public ChallengeDataService(IAzureService azureService,IFriendDataService _friendDataService)
        {
            _client = azureService.CreateOrGetAzureClient();
            friendDataService = _friendDataService;
            this.azureChallenge = _client.GetTable<AzureChallenge>();
        }

        #region Translators

    
        #endregion


        public async Task<Models.Challenge.Challenge> AddChallengeAsync(Models.Challenge.Challenge challenge)
        {
            try
            {
                if (challenge == null)
                    return null;
                AzureChallenge accountDto = ChallengeDataTranslator.TranslateChallenge(challenge);
                await azureChallenge.InsertAsync(accountDto);
                challenge = ChallengeDataTranslator.TranslateChallenge(accountDto);
                return challenge;
            }
            catch (System.Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId)
        {
            try
            {
                var challengeDTO = await azureChallenge.LookupAsync(challengeId);
                Models.Challenge.Challenge challenge = ChallengeDataTranslator.TranslateChallenge(challengeDTO);

                var account1 = await friendDataService.GetFriendAsync(challengeDTO.Account1);
                var account2 = await friendDataService.GetFriendAsync(challengeDTO.Account2);
                challenge.ChallengeAccounts = new System.Collections.ObjectModel.ObservableCollection<Journey.Models.Challenge.ChallengeAccount>();
                challenge.ChallengeAccounts.Add(new Journey.Models.Challenge.ChallengeAccount(account1));
                challenge.ChallengeAccounts.Add(new Journey.Models.Challenge.ChallengeAccount(account2));
                return challenge;
            }
            catch (System.Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> GetAccountChallengeAsync()
        {
            try
            {
                var account = _client.CurrentUser.UserId;
                var challengeDTO = await azureChallenge.Where(a => a.Status == true && (a.Account1 == account || a.Account2 == account)).ToListAsync();
                var accountChallenge = challengeDTO?.FirstOrDefault();
                if (accountChallenge == null)
                    return null;

                Models.Challenge.Challenge challenge = ChallengeDataTranslator.TranslateChallenge(accountChallenge);

                return challenge;
            }
            catch (System.Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }
    }
}
