using System.Collections.ObjectModel;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Challenge.Dto;

namespace Journey.Services.Buisness.Challenge.Translators
{
    public class ChallengeDataTranslator
    {
        public static AzureChallenge TranslateChallenge(Models.Challenge.Challenge challenge)
        {
            var postDto = new AzureChallenge();
            if (challenge != null)
            {
                postDto.Id = challenge.Id;
                postDto.Start = challenge.StartDate;
                postDto.End = challenge.EndDate;
                postDto.Terms = challenge.Terms;
                postDto.Account1 = challenge.ChallengeAccounts[0].Id;
                postDto.Account2 = challenge.ChallengeAccounts[1].Id;
                postDto.Status = challenge.IsActive;
            }

            return postDto;
        }

        public static Models.Challenge.Challenge TranslateChallenge(AzureChallenge challenge)
        {
            var postDto = new Models.Challenge.Challenge();

            if (challenge != null)
            {
                postDto.Id = challenge.Id;
                postDto.StartDate = challenge.Start;
                postDto.EndDate = challenge.End;
                postDto.Terms = challenge.Terms;
                postDto.IsActive = challenge.Status;
                postDto.ChallengeAccounts = new ObservableCollection<ChallengeAccount>();
                postDto.ChallengeAccounts.Add(
                    new ChallengeAccount(new Models.Account.Account {Id = challenge.Account1})
                );
                postDto.ChallengeAccounts.Add(
                    new ChallengeAccount(new Models.Account.Account {Id = challenge.Account2})
                );
            }

            return postDto;
        }
    }
}