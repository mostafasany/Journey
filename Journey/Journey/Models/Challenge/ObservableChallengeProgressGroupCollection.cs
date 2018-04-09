using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Journey.Models.Challenge
{
    public class ObservableChallengeProgressGroupCollection<T> : ObservableCollection<T>
    {
        public ObservableChallengeProgressGroupCollection(string key, List<T> items, Account.Account winnerAccountInKm, Account.Account winnerAccountInExercises) : base(items)
        {
            Key = key;
            WinnerAccountInKM = winnerAccountInKm;
            WinnerAccountInExercises = winnerAccountInExercises;
        }

        public string Key { get; set; }

        public Account.Account WinnerAccountInKM { get; set; }
        public Account.Account WinnerAccountInExercises { get; set; }
    }
}