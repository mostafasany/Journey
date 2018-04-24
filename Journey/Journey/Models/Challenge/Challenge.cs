using System;
using System.Collections.ObjectModel;
using Abstractions.Forms;
using Abstractions.Models;
using Journey.Models.Post;
using Prism.Mvvm;

namespace Journey.Models.Challenge
{
    public class Challenge : BindableBase
    {
        private ObservableCollection<ChallengeAccount> _challengeAccounts;

        private DateTime _endDate = DateTime.Now.AddMonths(6);
        private string _id;


        private Media _image;

        private int _interval;

        private bool _isActive;


        private ObservableCollection<PostBase> _posts;
        private Location _selectedLocation;

        private DateTime _startDate = DateTime.Now;

        private string _terms;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Terms
        {
            get => _terms;
            set => SetProperty(ref _terms, value);
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }


        public string StartDateFormated => _startDate.ToString("Y");

        public string EndDateFormated => _endDate.ToString("Y");


        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public ObservableCollection<ChallengeAccount> ChallengeAccounts
        {
            get => _challengeAccounts;
            set => SetProperty(ref _challengeAccounts, value);
        }

        public ObservableCollection<PostBase> Posts
        {
            get => _posts;
            set => SetProperty(ref _posts, value);
        }

        public int Interval
        {
            get => _interval;
            set => SetProperty(ref _interval, value);
        }

        public Media Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set => SetProperty(ref _selectedLocation, value);
        }
    }
}