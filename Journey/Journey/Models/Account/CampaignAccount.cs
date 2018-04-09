namespace Journey.Models.Account
{
    public class CampaignAccount : Account
    {
        private int exp;

        private bool liked;

        private int likes;

        public CampaignAccount(Account account)
        {
            SocialToken = account.SocialToken;
            SocialProvider = account.SocialProvider;
            Email = account.Email;
            Gender = account.Gender;
            FirstName = account.FirstName;
            LastName = account.LastName;
            Id = account.Id;
            Image = account.Image;
        }

        public int Exp
        {
            get => exp;
            set => SetProperty(ref exp, value);
        }

        public int Likes
        {
            get => likes;
            set => SetProperty(ref likes, value);
        }

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