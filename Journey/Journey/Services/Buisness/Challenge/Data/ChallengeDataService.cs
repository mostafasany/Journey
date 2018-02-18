using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Account;
using Journey.Models.Challenge;
using Journey.Services.Azure;
using Microsoft.WindowsAzure.MobileServices;
using Tawasol.Azure.Models;

namespace Tawasol.Services.Data
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


        public async Task<Challenge> AddChallengeAsync(Challenge challenge)
        {
            try
            {
                if (challenge == null)
                    return null;
                AzureChallenge accountDto = Journey.Services.Buisness.Challenge.Dto.ChallengeDataTranslator.TranslateChallenge(challenge);
                await azureChallenge.InsertAsync(accountDto);
                return challenge;
            }
            catch (System.Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<Challenge> GetChallengeAsync(string challengeId)
        {
            try
            {
                var challengeDTO = await azureChallenge.LookupAsync(challengeId);
                Challenge challenge = Journey.Services.Buisness.Challenge.Dto.ChallengeDataTranslator.TranslateChallenge(challengeDTO);

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

        public async Task<Challenge> GetAccountChallengeAsync()
        {
            try
            {
                var account = _client.CurrentUser.UserId;
                var challengeDTO = await azureChallenge.Where(a => a.Status == true && (a.Account1 == account || a.Account2 == account)).ToListAsync();
                var accountChallenge = challengeDTO?.FirstOrDefault();
                if (accountChallenge == null)
                    return null;

                Challenge challenge = Journey.Services.Buisness.Challenge.Dto.ChallengeDataTranslator.TranslateChallenge(accountChallenge);

                return challenge;
            }
            catch (System.Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }
    }
}
