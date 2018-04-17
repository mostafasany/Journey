using System;
using Abstractions.Models;
using Prism.Mvvm;

namespace Journey.Models.Challenge
{
    public class ChallengeActivityLog : BindableBase
    {
        public string Challenge;

        private Account.Account _account;
        private DateTime _datetTime;

        private string _id;

        private bool _mine;

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
                RaisePropertyChanged(nameof(FormatedDate));
            }
        }

        public string FormatedDate => _datetTime.ToString("M");

        public Account.Account Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }

        public bool Mine
        {
            get => _mine;
            set => SetProperty(ref _mine, value);
        }
    }

    public class ChallengeWorkoutActivityLog : ChallengeActivityLog
    {
        private Location _location;

        public Location Location
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

    public class ChallengeKcalActivityLog : ChallengeActivityLog
    {
        private double _kcal;

        public double Kcal
        {
            get => _kcal;
            set => SetProperty(ref _kcal, value);
        }
    }
}