namespace Journey.Models.Account
{
    public class CampaignAccount : Account
    {
        private int _exp;

        private bool _liked;

        private int _likes;

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
            get => _exp;
            set => SetProperty(ref _exp, value);
        }

        public int Likes
        {
            get => _likes;
            set => SetProperty(ref _likes, value);
        }

        public bool Liked
        {
            get => _liked;
            set
            {
                SetProperty(ref _liked, value);
                RaisePropertyChanged(nameof(NotLiked));
            }
        }


        public bool NotLiked => !Liked;

        public string FormatedExpPoint => $"{Exp} Exp";
    }
}