using System.Collections.Generic;

namespace Journey.Models.Post
{
    public class Post : PostBase
    {
        private PostActivity activity;

        private PostActivity location;
        private List<ScaleMeasurment> measuremnts;

        public List<ScaleMeasurment> Measuremnts
        {
            get => measuremnts;
            set => SetProperty(ref measuremnts, value);
        }


        public bool HasMeasuremnts => Measuremnts != null && Measuremnts.Count > 0;

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