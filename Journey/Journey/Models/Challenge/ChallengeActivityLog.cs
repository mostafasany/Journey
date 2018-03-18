using System;
using Journey.Models.Post;
using Prism.Mvvm;

namespace Journey.Models.Challenge
{
    public class ChallengeActivityLog : BindableBase
    {
        private DateTime _datetTime;

        private Account.Account _account;

        private string _id;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public DateTime DatetTime
        {
            get => _datetTime;
            set
            {
                _datetTime = value;
                RaisePropertyChanged();
                RaisePropertyChanged("FormatedDate");
            }
        }

        public string FormatedDate => _datetTime.ToString("M");

        public Account.Account Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }

        private bool _mine;

        public bool Mine
        {
            get => _mine;
            set => SetProperty(ref _mine, value);
        }
    }

    public class ChallengeWorkoutActivityLog : ChallengeActivityLog
    {
        private string _location;

        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }
    }

    public class ChallengeKmActivityLog : ChallengeActivityLog
    {
        private double _km;

        public double KM
        {
            get => _km;
            set => SetProperty(ref _km, value);
        }
    }
}