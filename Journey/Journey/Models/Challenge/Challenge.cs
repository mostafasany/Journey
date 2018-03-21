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
        private ObservableCollection<ChallengeAccount> challengeAccounts;

        private DateTime endDate = DateTime.Now.AddMonths(6);
        private string id;


        private Media image;

        private int interval;

        private bool isActive;


        private ObservableCollection<PostBase> posts;

        private DateTime startDate = DateTime.Now;

        private string terms;

        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string Terms
        {
            get => terms;
            set => SetProperty(ref terms, value);
        }

        public DateTime EndDate
        {
            get => endDate;
            set => SetProperty(ref endDate, value);
        }

        public DateTime StartDate
        {
            get => startDate;
            set => SetProperty(ref startDate, value);
        }


        public string StartDateFormated => startDate.ToString("Y");

        public string EndDateFormated => endDate.ToString("Y");


        public bool IsActive
        {
            get => isActive;
            set => SetProperty(ref isActive, value);
        }

        public ObservableCollection<ChallengeAccount> ChallengeAccounts
        {
            get => challengeAccounts;
            set => SetProperty(ref challengeAccounts, value);
        }

        public ObservableCollection<PostBase> Posts
        {
            get => posts;
            set => SetProperty(ref posts, value);
        }

        public int Interval
        {
            get => interval;
            set => SetProperty(ref interval, value);
        }

        public Media Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        private Location _selectedLocation;

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set => SetProperty(ref _selectedLocation, value);
        }
    }
}