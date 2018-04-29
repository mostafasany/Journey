using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Abstractions.Exceptions;
using Abstractions.Models;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Challenge.Dto;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Challenge.Translators
{
    public class ChallengeDataTranslator
    {
        public static AzureChallenge TranslateChallenge(Models.Challenge.Challenge challenge, string account)
        {
            try
            {
                var challengeDto = new AzureChallenge();
                if (challenge != null)
                {
                    challengeDto.Id = challenge.Id;
                    challengeDto.Start = challenge.StartDate;
                    challengeDto.End = challenge.EndDate;
                    challengeDto.Terms = challenge.Terms;
                    challengeDto.Account1 = challenge.ChallengeAccounts[0].Id;
                    challengeDto.Account2 = challenge.ChallengeAccounts[1].Id;
                    challengeDto.Status = challenge.IsActive;
                    if (challenge.SelectedLocation != null)
                    {
                        if (account == challenge.ChallengeAccounts[0].Id)
                        {
                            challengeDto.Location1 = JsonConvert.SerializeObject(challenge.SelectedLocation);
                            challengeDto.Location2 = JsonConvert.SerializeObject(challenge.ChallengerLocation);
                        }
                        else if (account == challenge.ChallengeAccounts[1].Id)
                        {
                            challengeDto.Location2 = JsonConvert.SerializeObject(challenge.SelectedLocation);
                            challengeDto.Location1 = JsonConvert.SerializeObject(challenge.ChallengerLocation);
                        }
                    }
                }

                return challengeDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Challenge", ex.InnerException);
            }

        }

        public static List<Models.Challenge.Challenge> TranslateChallenges(List<AzureChallenge> challenges, string account)
        {
            try
            {
                return challenges == null ? new List<Models.Challenge.Challenge>() : challenges.Select(a => TranslateChallenge(a, account)).ToList();
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Challenge", ex.InnerException);
            }
        }

        public static Models.Challenge.Challenge TranslateChallenge(AzureChallenge challenge, string account)
        {
            try
            {
                var challengeDto = new Models.Challenge.Challenge();

                if (challenge != null)
                {
                    challengeDto.Id = challenge.Id;
                    challengeDto.StartDate = challenge.Start;
                    challengeDto.EndDate = challenge.End;
                    challengeDto.Terms = challenge.Terms;
                    challengeDto.IsActive = challenge.Status;
                    challengeDto.ChallengeAccounts = new ObservableCollection<ChallengeAccount>();
                    challengeDto.ChallengeAccounts.Add(
                        new ChallengeAccount(new Models.Account.Account { Id = challenge.Account1 })
                    );
                    challengeDto.ChallengeAccounts.Add(
                        new ChallengeAccount(new Models.Account.Account { Id = challenge.Account2 })
                    );
                    if (account == challenge.Account1)
                    {
                        if (challenge.Location1 != null)
                            challengeDto.SelectedLocation = JsonConvert.DeserializeObject<Location>(challenge.Location1);
                        if (challenge.Location2 != null)
                            challengeDto.ChallengerLocation = JsonConvert.DeserializeObject<Location>(challenge.Location2);

                    }
                    else if (account == challenge.Account2)
                    {
                        if (challenge.Location2 != null)
                            challengeDto.SelectedLocation = JsonConvert.DeserializeObject<Location>(challenge.Location2);
                        if (challenge.Location1 != null)
                            challengeDto.ChallengerLocation = JsonConvert.DeserializeObject<Location>(challenge.Location1);

                    }
                }

                return challengeDto;

            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Challenge", ex.InnerException);
            }

        }
    }
}