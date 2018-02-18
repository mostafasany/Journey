using System;
using System.Collections.ObjectModel;
using Journey.Models.Post;
using Prism.Mvvm;

namespace Journey.Models.Challenge
{
    public class Challenge : BindableBase
    {
        private string id;
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string terms;
        public string Terms
        {
            get => terms;
            set => SetProperty(ref terms, value);
        }

        private DateTime endDate = DateTime.Now.AddMonths(6);
        public DateTime EndDate
        {
            get => endDate;
            set => SetProperty(ref endDate, value);
        }

        private DateTime startDate = DateTime.Now;
        public DateTime StartDate
        {
            get => startDate;
            set => SetProperty(ref startDate, value);
        }

        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set => SetProperty(ref isActive, value);
        }

        private ObservableCollection<ChallengeAccount> challengeAccounts;
        public ObservableCollection<ChallengeAccount> ChallengeAccounts
        {
            get => challengeAccounts;
            set => SetProperty(ref challengeAccounts, value);
        }


        private ObservableCollection<PostBase> posts;
        public ObservableCollection<PostBase> Posts
        {
            get => posts;
            set => SetProperty(ref posts, value);
        }

        private int interval;
        public int Interval
        {
            get => interval;
            set => SetProperty(ref interval, value);
        }


        private Media image;
        public Media Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }
    }

}
