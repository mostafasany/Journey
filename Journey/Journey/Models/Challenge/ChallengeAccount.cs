namespace Journey.Models.Challenge
{
    public class ChallengeAccount : Account.Account
    {
        private int exp;

        private bool liked;

        private int likes;

        private int numberExercise;

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
            get => exp;
            set
            {
                exp = value;
                RaisePropertyChanged();
            }
        }

        public int Likes
        {
            get => likes;
            set
            {
                likes = value;
                RaisePropertyChanged();
            }
        }


        public bool Liked
        {
            get => liked;
            set
            {
                liked = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(NotLiked));
            }
        }


        public bool NotLiked => !Liked;

        public string FormatedExpPoint => string.Format("{0} {1}", Exp, "Exp");
    }
}