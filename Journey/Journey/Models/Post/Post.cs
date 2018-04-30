namespace Journey.Models.Post
{
    public class Post : PostBase
    {
        private PostActivity activity;

        private PostActivity location;

        public PostActivity Location
        {
            get => location;
            set
            {
                location = value;
                RaisePropertyChanged();
            }
        }

        public bool HasLocation => Location != null;

        public PostActivity Activity
        {
            get => activity;
            set
            {
                activity = value;
                RaisePropertyChanged();
            }
        }

        public bool HasActivity => Activity != null;
    }
}