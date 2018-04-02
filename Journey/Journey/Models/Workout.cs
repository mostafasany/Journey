using Prism.Mvvm;

namespace Journey.Models
{
    public class Workout : BindableBase
    {
        private string id;

        private string image;

        private string title;

        private string weight;
        private string reps;

        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public string Weight
        {
            get => weight;
            set => SetProperty(ref weight, value);
        }

        public string Reps
        {
            get => reps;
            set => SetProperty(ref reps, value);
        }


    }
}