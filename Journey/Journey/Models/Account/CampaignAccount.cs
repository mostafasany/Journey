using System.Collections.Generic;
using Abstractions.Models;
namespace Tawasol.Models
{
    public class CampaignAccount : Account
    {
        public CampaignAccount(Account account)
        {
            this.SocialToken = account.SocialToken;
            this.SocialProvider = account.SocialProvider;
            this.Email = account.Email;
            this.Gender = account.Gender;
            FirstName = account.FirstName;
            LastName = account.LastName;
            Id = account.Id;
            Image = account.Image;
        }

        private int exp;
        public int Exp
        {
            get => exp;
            set => SetProperty(ref exp, value);
        }

        private int likes;
        public int Likes
        {
            get => likes;
            set => SetProperty(ref likes, value);
        }

        private bool liked;
        public bool Liked
        {
            get => liked;
            set
            {
                SetProperty(ref liked, value);
                RaisePropertyChanged(nameof(NotLiked));
            }
        }


        public bool NotLiked => !Liked;

        public string FormatedExpPoint => $"{Exp} {"Exp"}";
    }
}