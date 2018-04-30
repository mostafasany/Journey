using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Challenge;
using Journey.Services.Azure;
using Journey.Services.Buisness.Account.Data;
using Journey.Services.Buisness.Challenge.Dto;
using Journey.Services.Buisness.Challenge.Translators;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Challenge.Data
{
    public class ChallengeDataService : IChallengeDataService
    {
        private readonly IAccountDataService _accountDataService;
        private readonly IMobileServiceTable<AzureChallenge> _azureChallenge;
        private readonly MobileServiceClient _client;

        public ChallengeDataService(IAzureService azureService, IAccountDataService accountDataService)
        {
            _client = azureService.CreateOrGetAzureClient();
            _accountDataService = accountDataService;
            _azureChallenge = _client.GetTable<AzureChallenge>();
        }

        public async Task<Models.Challenge.Challenge> AddChallengeAsync(Models.Challenge.Challenge challenge)
        {
            try
            {
                if (challenge == null)
                    return null;
                AzureChallenge accountDto = ChallengeDataTranslator.TranslateChallenge(challenge, _client.CurrentUser.UserId);
                await _azureChallenge.InsertAsync(accountDto);
                challenge = ChallengeDataTranslator.TranslateChallenge(accountDto, _client.CurrentUser.UserId);
                return challenge;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> EditChallengeAsync(Models.Challenge.Challenge challenge)
        {
            try
            {
                if (challenge == null)
                    return null;
                AzureChallenge accountDto = ChallengeDataTranslator.TranslateChallenge(challenge, _client.CurrentUser.UserId);
                await _azureChallenge.UpdateAsync(accountDto);
                challenge = ChallengeDataTranslator.TranslateChallenge(accountDto, _client.CurrentUser.UserId);
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
                AzureChallenge challengeDto = await _azureChallenge.LookupAsync(challengeId);

                Models.Challenge.Challenge challenge = await GetChallengeWithChallengersAsync(challengeDto);

                return challenge;
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound) return null;
                throw new DataServiceException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> HasChallengeAsync(string friend)
        {
            try
            {
                List<AzureChallenge> challengeDto = await _azureChallenge
                    .Where(a => a.Account1 == friend || a.Account2 == friend).ToListAsync();
                AzureChallenge accountChallenge = challengeDto?.FirstOrDefault();
                if (accountChallenge == null)
                    return null;

                Models.Challenge.Challenge challenge = ChallengeDataTranslator.TranslateChallenge(accountChallenge, _client.CurrentUser.UserId);

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
                AzureChallenge accountDto = ChallengeDataTranslator.TranslateChallenge(challenge, _client.CurrentUser.UserId);
                await _azureChallenge.UpdateAsync(accountDto);
                challenge = ChallengeDataTranslator.TranslateChallenge(accountDto, _client.CurrentUser.UserId);
                return challenge;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<List<Models.Challenge.Challenge>> GetChallengeRequestsAsync()
        {
            try
            {
                List<AzureChallenge> challengesDto = await _azureChallenge.Where(a => a.Account2 == _client.CurrentUser.UserId && !a.Status).ToListAsync();
                var challenges = new List<Models.Challenge.Challenge>();
                foreach (AzureChallenge item in challengesDto)
                {
                    Models.Challenge.Challenge challenge = await GetChallengeWithChallengersAsync(item);
                    challenges.Add(challenge);
                }

                return challenges;
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound) return null;
                throw new DataServiceException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        private async Task<Models.Challenge.Challenge> GetChallengeWithChallengersAsync(AzureChallenge challengeDto)
        {
            try
            {
                Models.Challenge.Challenge challenge = ChallengeDataTranslator.TranslateChallenge(challengeDto, _client.CurrentUser.UserId);
                Models.Account.Account account1 = await _accountDataService.GetAccontAsync(challengeDto.Account1);
                Models.Account.Account account2 = await _accountDataService.GetAccontAsync(challengeDto.Account2);
                challenge.ChallengeAccounts = new ObservableCollection<ChallengeAccount>();
                challenge.ChallengeAccounts.Add(
                    new ChallengeAccount(account1));
                challenge.ChallengeAccounts.Add(
                    new ChallengeAccount(account2));
                return challenge;
            }

            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }
    }
}