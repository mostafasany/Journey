namespace Tawasol.Models
{
    public class ChallengeAccount : Account
    {
        public ChallengeAccount(Account account)
        {
            this.SocialToken = account.SocialToken;
            this.SocialProvider = account.SocialProvider;
            this.Email = account.Email;
            this.Gender = account.Gender;
            this.FirstName = account.FirstName;
            this.LastName = account.LastName;
            this.Id = account.Id;
            this.Image = account.Image;
        }

        private int exp;
        public int Exp
        {
            get
            {
                return exp;
            }
            set
            {
                exp = value;
                RaisePropertyChanged();
            }
        }

        private int likes;
        public int Likes
        {
            get
            {
                return likes;
            }
            set
            {
                likes = value;
                RaisePropertyChanged();
            }
        }

        private bool liked;
        public bool Liked
        {
            get
            {
                return liked;
            }
            set
            {
                liked = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(NotLiked));
            }
        }


        public bool NotLiked
        {
            get
            {
                return !Liked;
            }
        }

        public string FormatedExpPoint
        {
            get
            {
                return string.Format("{0} {1}", Exp, "Exp");
            }
        }
    }
}