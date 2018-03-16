using System;
using System.Collections.Generic;
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
        private readonly IMobileServiceTable<AzureChallenge> _azureChallenge;
        private readonly MobileServiceClient _client;
        private readonly IFriendDataService _friendDataService;

        public ChallengeDataService(IAzureService azureService, IFriendDataService friendDataService)
        {
            _client = azureService.CreateOrGetAzureClient();
            _friendDataService = friendDataService;
            _azureChallenge = _client.GetTable<AzureChallenge>();
        }


        public async Task<Models.Challenge.Challenge> AddChallengeAsync(Models.Challenge.Challenge challenge)
        {
            try
            {
                if (challenge == null)
                    return null;
                AzureChallenge accountDto = ChallengeDataTranslator.TranslateChallenge(challenge);
                await _azureChallenge.InsertAsync(accountDto);
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
                AzureChallenge challengeDto = await _azureChallenge.LookupAsync(challengeId);
                //if (challengeDTO.Status == false)
                //return null;

                Models.Challenge.Challenge challenge = ChallengeDataTranslator.TranslateChallenge(challengeDto);
                Models.Account.Account account1 = await _friendDataService.GetFriendAsync(challengeDto.Account1);
                Models.Account.Account account2 = await _friendDataService.GetFriendAsync(challengeDto.Account2);
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

        public async Task<Models.Challenge.Challenge> CheckAccountHasChallengeAsync()
        {
            try
            {
                string account = _client.CurrentUser.UserId;
                List<AzureChallenge> challengeDto = await _azureChallenge
                    .Where(a => a.Account1 == account || a.Account2 == account).ToListAsync();
                AzureChallenge accountChallenge = challengeDto?.FirstOrDefault();
                if (accountChallenge == null)
                    return null;

                Models.Challenge.Challenge challenge = ChallengeDataTranslator.TranslateChallenge(accountChallenge);

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
                AzureChallenge accountDto = ChallengeDataTranslator.TranslateChallenge(challenge);
                await _azureChallenge.UpdateAsync(accountDto);
                challenge = ChallengeDataTranslator.TranslateChallenge(accountDto);
                return challenge;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex.Message, ex);
            }
        }

        public async Task<List<ChallengeProgress>> GetChallengePorgessAsync(string challengeId)
        {
            var status = new List<ChallengeProgress>
            {
                new ChallengeProgress
                {
                    FirstName = "Mostafa",
                    LastName = "Khodeir",
                    DatetTime = DateTime.Now,
                    Exercises = 1,
                    Km = 3000
                },
                new ChallengeProgress
                {
                    FirstName = "Mostafa",
                    LastName = "Khodeir",

                    DatetTime = DateTime.Now,
                    Exercises = 1,
                    Km = 2000
                },
                new ChallengeProgress
                {
                    FirstName = "Mostafa",
                    LastName = "Khodeir",

                    DatetTime = DateTime.Now,
                    Exercises = 1,
                    Km = 1000
                },
                new ChallengeProgress
                {
                    FirstName = "Heba",
                    LastName = "El-Liethy",

                    DatetTime = DateTime.Now,
                    Exercises = 4,
                    Km = 3500
                },
                new ChallengeProgress
                {
                    FirstName = "Mostafa",
                    LastName = "Khodeir",

                    DatetTime = DateTime.Now.AddMonths(1),
                    Exercises = 2,
                    Km = 3500
                },
                new ChallengeProgress
                {
                    FirstName = "Heba",
                    LastName = "El-Liethy",

                    DatetTime = DateTime.Now.AddMonths(1),
                    Exercises = 10,
                    Km = 3500
                },
                new ChallengeProgress
                {
                    FirstName = "Mostafa",
                    LastName = "Khodeir",

                    DatetTime = DateTime.Now.AddMonths(2),
                    Exercises = 10,
                    Km = 4000
                },
                new ChallengeProgress
                {
                    FirstName = "Mostafa",
                    LastName = "Khodeir",

                    DatetTime = DateTime.Now.AddMonths(3),
                    Exercises = 1,
                    Km = 100
                },
                new ChallengeProgress
                {
                    FirstName = "Mostafa",
                    LastName = "Khodeir",

                    DatetTime = DateTime.Now.AddMonths(3),
                    Exercises = 1,
                    Km = 100
                },
                new ChallengeProgress
                {
                    FirstName = "Mostafa",
                    LastName = "Khodeir",

                    DatetTime = DateTime.Now.AddMonths(3),
                    Exercises = 1,
                    Km = 100
                },
                new ChallengeProgress
                {
                    FirstName = "Mostafa",
                    LastName = "Khodeir",

                    DatetTime = DateTime.Now.AddMonths(3),
                    Exercises = 1,
                    Km = 100
                },
                new ChallengeProgress
                {
                    FirstName = "Heba",
                    LastName = "El-Liethy",

                    DatetTime = DateTime.Now.AddMonths(3),
                    Exercises = 1,
                    Km = 50
                },
                new ChallengeProgress
                {
                    FirstName = "Heba",
                    LastName = "El-Liethy",

                    DatetTime = DateTime.Now.AddMonths(3),
                    Exercises = 10,
                    Km = 50
                },
                new ChallengeProgress
                {
                    FirstName = "Heba",
                    LastName = "El-Liethy",

                    DatetTime = DateTime.Now.AddMonths(3),
                    Exercises = 1,
                    Km = 50
                },
                new ChallengeProgress
                {
                    FirstName = "Heba",
                    LastName = "El-Liethy",

                    DatetTime = DateTime.Now.AddMonths(3),
                    Exercises = 1,
                    Km = 450
                }
            };


            return status;
        }
    }
}