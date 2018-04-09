namespace Journey.Models.Challenge
{
    public class ChallengeAccount : Account.Account
    {
        private int _exp;

        private bool _liked;

        private int _likes;

        public ChallengeAccount(Account.Account account)
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
            set
            {
                _exp = value;
                RaisePropertyChanged();
            }
        }

        public int Likes
        {
            get => _likes;
            set
            {
                _likes = value;
                RaisePropertyChanged();
            }
        }


        public bool Liked
        {
            get => _liked;
            set
            {
                _liked = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(NotLiked));
            }
        }


        public bool NotLiked => !Liked;

        public string FormatedExpPoint => string.Format("{0} {1}", Exp, "Exp");
    }
}