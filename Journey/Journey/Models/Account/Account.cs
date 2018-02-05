using System;
using System.Collections.Generic;
using Abstractions.Models;
using Prism.Mvvm;

namespace Tawasol.Models
{
    public class Account : BindableBase
    {
        public string Token { get; set; }
        public string SocialToken { get; set; }
        public string SID { get; set; }
        public string SocialProvider { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string ChallengeId { get; set; }


        private string id;
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string status;
        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }


        private string firstName;
        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string lastName;
        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        private bool following;
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

        public bool NotFollowing
        {
            get => !Following;
        }

        public string Name => $"{FirstName} {LastName}";

        private Media image;
        public Media Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        private AccountGoal accountGoal;
        public AccountGoal AccountGoal
        {
            get => accountGoal;
            set => SetProperty(ref accountGoal, value);
        }

        private IEnumerable<Media> mediaList;
        public IEnumerable<Media> MediaList
        {
            get => mediaList;
            set => SetProperty(ref mediaList, value);
        }

        public bool HasNotActiveChallenge
        {
            get
            {
                return string.IsNullOrEmpty(ChallengeId);
            }
        }


        public bool HasActiveChallenge
        {
            get
            {
                return !string.IsNullOrEmpty(ChallengeId);
            }
        }


        public override string ToString()
        {
            return Name;
        }
    }

    public class AccountGoal : BindableBase
    {
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


        DateTime start;
        public DateTime Start
        {
            get => start;
            set => SetProperty(ref start, value);
        }

        DateTime end;
        public DateTime End
        {
            get => end;
            set => SetProperty(ref end, value);
        }

        private double goal;
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

