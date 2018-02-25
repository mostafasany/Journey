using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Challenge;
using Journey.Services.Azure;
using Journey.Services.Buisness.Challenge.Dto;
using Journey.Services.Buisness.Challenge.Translators;
using Journey.Services.Buisness.Friend.Data;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Challenge.Data
{
    public class ChallengeDataService : IChallengeDataService
    {
        private readonly MobileServiceClient _client;
        private readonly IMobileServiceTable<AzureChallenge> azureChallenge;
        private readonly IFriendDataService friendDataService;

        public ChallengeDataService(IAzureService azureService, IFriendDataService _friendDataService)
        {
            _client = azureService.CreateOrGetAzureClient();
            friendDataService = _friendDataService;
            azureChallenge = _client.GetTable<AzureChallenge>();
        }


        public async Task<Models.Challenge.Challenge> AddChallengeAsync(Models.Challenge.Challenge challenge)
        {
            try
            {
                if (challenge == null)
                    return null;
                var accountDto = ChallengeDataTranslator.TranslateChallenge(challenge);
                await azureChallenge.InsertAsync(accountDto);
                challenge = ChallengeDataTranslator.TranslateChallenge(accountDto);
                return challenge;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId)
        {
            try
            {
                var challengeDTO = await azureChallenge.LookupAsync(challengeId);
                //if (challengeDTO.Status == false)
                    //return null;
                
                var challenge = ChallengeDataTranslator.TranslateChallenge(challengeDTO);
                var challenger1 = challenge.ChallengeAccounts.FirstOrDefault();
                var challenger2 = challenge.ChallengeAccounts.LastOrDefault();
                var account1 = await friendDataService.GetFriendAsync(challengeDTO.Account1);
                var account2 = await friendDataService.GetFriendAsync(challengeDTO.Account2);
                challenge.ChallengeAccounts = new ObservableCollection<ChallengeAccount>();
                challenge.ChallengeAccounts.Add(new ChallengeAccount(account1){ NumberExercise = challenger1.NumberExercise });
                challenge.ChallengeAccounts.Add(new ChallengeAccount(account2){ NumberExercise = challenger2.NumberExercise });
                return challenge;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> CheckAccountHasChallengeAsync()
        {
            try
            {
                var account = _client.CurrentUser.UserId;
                var challengeDTO = await azureChallenge
                    .Where(a => (a.Account1 == account || a.Account2 == account)).ToListAsync();
                var accountChallenge = challengeDTO?.FirstOrDefault();
                if (accountChallenge == null)
                    return null;

                var challenge = ChallengeDataTranslator.TranslateChallenge(accountChallenge);

                return challenge;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> UpdateChallengeAsync(Models.Challenge.Challenge challenge)
        {
            try
            {
                if (challenge == null)
                    return null;
                var accountDto = ChallengeDataTranslator.TranslateChallenge(challenge);
                await azureChallenge.UpdateAsync(accountDto);
                challenge = ChallengeDataTranslator.TranslateChallenge(accountDto);
                return challenge;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }
    }
}