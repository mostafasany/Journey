using System;
using System.Collections.Generic;
using Prism.Mvvm;

namespace Journey.Models.Challenge
{
    public class KeyGroupedChallengeProgress : BindableBase
    {
        string _key;
        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        List<AccountChallengeProgress> _accounts;
        public List<AccountChallengeProgress> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }
    }

    public class AccountChallengeProgress : BindableBase
    {
        string _id;
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        Account.Account _account;
        public Account.Account Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }

        int _totalExercises;
        public int TotalExercises
        {
            get => _totalExercises;
            set => SetProperty(ref _totalExercises, value);
        }

        double _totalkm;
        public double TotalKm
        {
            get => _totalkm;
            set => SetProperty(ref _totalkm, value);
        }
    }
    public class ChallengeProgress : Account.Account
    {
        private string _id;
        public new string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        DateTime _datetTime;
        public DateTime DatetTime
        {
            get => _datetTime;
            set => SetProperty(ref _datetTime, value);
        }

        int _exercises;
        public int Exercises
        {
            get => _exercises;
            set => SetProperty(ref _exercises, value);
        }

        double _km;
        public double Km
        {
            get => _km;
            set => SetProperty(ref _km, value);
        }


    }
}
