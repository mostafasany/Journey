using System.Collections.ObjectModel;
using Abstractions.Models;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Challenge.Dto;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Challenge.Translators
{
    public class ChallengeDataTranslator
    {
        public static AzureChallenge TranslateChallenge(Models.Challenge.Challenge challenge)
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
                    challengeDto.Location1 = JsonConvert.SerializeObject(challenge.SelectedLocation);
            }

            return challengeDto;
        }

        public static Models.Challenge.Challenge TranslateChallenge(AzureChallenge challenge, string account)
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
                }
                else if (account == challenge.Account2)
                {
                    if (challenge.Location2 != null)
                        challengeDto.SelectedLocation = JsonConvert.DeserializeObject<Location>(challenge.Location2);
                }
            }

            return challengeDto;
        }
    }
}