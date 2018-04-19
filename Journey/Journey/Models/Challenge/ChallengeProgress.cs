using System;
using System.Collections.Generic;
using Prism.Mvvm;

namespace Journey.Models.Challenge
{
    public class KeyGroupedChallengeProgress : BindableBase
    {
        private List<AccountChallengeProgress> _accounts;
        private string _key;

        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public List<AccountChallengeProgress> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }
    }

    public class AccountChallengeProgress : BindableBase
    {
        private Account.Account _account;
        private string _id;

        private int _totalExercises;

        private double _totalkm;
        private double _totalkcal;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public Account.Account Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }

        public int TotalExercises
        {
            get => _totalExercises;
            set => SetProperty(ref _totalExercises, value);
        }

        public double TotalKm
        {
            get => _totalkm;
            set => SetProperty(ref _totalkm, value);
        }


        public double TotalKcal
        {
            get => _totalkcal;
            set => SetProperty(ref _totalkcal, value);
        }
    }

    public class ChallengeProgress : Account.Account
    {
        private DateTime _datetTime;

        private int _exercises;
        private string _id;

        private double _km;

        public new string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public DateTime DatetTime
        {
            get => _datetTime;
            set => SetProperty(ref _datetTime, value);
        }

        public int Exercises
        {
            get => _exercises;
            set => SetProperty(ref _exercises, value);
        }

        public double Km
        {
            get => _km;
            set => SetProperty(ref _km, value);
        }
    }
}