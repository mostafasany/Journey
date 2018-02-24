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
                postDto.Account1Exercise = challenge.ChallengeAccounts[0].NumberExercise;
                postDto.Account2Exercise = challenge.ChallengeAccounts[1].NumberExercise;
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
                    new ChallengeAccount(new Models.Account.Account { Id = challenge.Account1 }) { NumberExercise = challenge.Account1Exercise });
                postDto.ChallengeAccounts.Add(
                    new ChallengeAccount(new Models.Account.Account { Id = challenge.Account2 }) { NumberExercise = challenge.Account2Exercise });
            }
            return postDto;
        }
    }
}