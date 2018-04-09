using System;
using System.Collections.Generic;
using Abstractions.Forms;
using Prism.Mvvm;

namespace Journey.Models.Account
{
    public class Account : BindableBase
    {
        private AccountGoal _accountGoal;


        private string _firstName;

        private bool _following;


        private string _id;

        private Media _image;

        private string _lastName;

        private IEnumerable<Media> _mediaList;

        private string _status;
        public string Token { get; set; }
        public string SocialToken { get; set; }
        public string SID { get; set; }
        public string SocialProvider { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string ChallengeId { get; set; }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public bool Following
        {
            get => _following;
            set
            {
                SetProperty(ref _following, value);
                RaisePropertyChanged(nameof(NotFollowing));
            }
        }

        //In Case you have friend
        public string FollowingId { get; set; }

        public bool NotFollowing => !Following;

        public string Name => $"{FirstName} {LastName}";

        public Media Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public AccountGoal AccountGoal
        {
            get => _accountGoal;
            set => SetProperty(ref _accountGoal, value);
        }

        public IEnumerable<Media> MediaList
        {
            get => _mediaList;
            set => SetProperty(ref _mediaList, value);
        }

        public bool HasNotActiveChallenge => string.IsNullOrEmpty(ChallengeId);


        public bool HasActiveChallenge => !string.IsNullOrEmpty(ChallengeId);


        public override string ToString() => Name;
    }

    public class AccountGoal : BindableBase
    {
        private DateTime _end;

        private double _goal;


        private DateTime _start;
        private double _weight;

        public double Weight
        {
            get => _weight;
            set
            {
                SetProperty(ref _weight, value);
                RaisePropertyChanged(nameof(WeightWithUnit));
            }
        }

        public string WeightWithUnit => $"{Weight} KG";

        public DateTime Start
        {
            get => _start;
            set => SetProperty(ref _start, value);
        }

        public DateTime End
        {
            get => _end;
            set => SetProperty(ref _end, value);
        }

        public double Goal
        {
            get => _goal;
            set
            {
                SetProperty(ref _goal, value);
                RaisePropertyChanged(nameof(GoalWithUnit));
            }
        }

        public string GoalWithUnit => $"{Goal} KG";
    }
}