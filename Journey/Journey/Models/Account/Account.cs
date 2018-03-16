using System;
using System.Collections.Generic;
using Abstractions.Forms;
using Prism.Mvvm;

namespace Journey.Models.Account
{
    public class Account : BindableBase
    {
        private AccountGoal accountGoal;


        private string firstName;

        private bool following;


        private string id;

        private Media image;

        private string lastName;

        private IEnumerable<Media> mediaList;

        private string status;
        public string Token { get; set; }
        public string SocialToken { get; set; }
        public string SID { get; set; }
        public string SocialProvider { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string ChallengeId { get; set; }

        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        public bool Following
        {
            get => following;
            set
            {
                SetProperty(ref following, value);
                RaisePropertyChanged(nameof(NotFollowing));
            }
        }

        //In Case you have friend
        public string FollowingId { get; set; }

        public bool NotFollowing => !Following;

        public string Name => $"{FirstName} {LastName}";

        public Media Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public AccountGoal AccountGoal
        {
            get => accountGoal;
            set => SetProperty(ref accountGoal, value);
        }

        public IEnumerable<Media> MediaList
        {
            get => mediaList;
            set => SetProperty(ref mediaList, value);
        }

        public bool HasNotActiveChallenge => string.IsNullOrEmpty(ChallengeId);


        public bool HasActiveChallenge => !string.IsNullOrEmpty(ChallengeId);


        public override string ToString() => Name;
    }

    public class AccountGoal : BindableBase
    {
        private DateTime end;

        private double goal;


        private DateTime start;
        private double weight;

        public double Weight
        {
            get => weight;
            set
            {
                SetProperty(ref weight, value);
                RaisePropertyChanged(nameof(WeightWithUnit));
            }
        }

        public string WeightWithUnit => $"{Weight} KG";

        public DateTime Start
        {
            get => start;
            set => SetProperty(ref start, value);
        }

        public DateTime End
        {
            get => end;
            set => SetProperty(ref end, value);
        }

        public double Goal
        {
            get => goal;
            set
            {
                SetProperty(ref goal, value);
                RaisePropertyChanged(nameof(GoalWithUnit));
            }
        }

        public string GoalWithUnit => $"{Goal} KG";
    }
}