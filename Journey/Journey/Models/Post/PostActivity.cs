using Prism.Mvvm;

namespace Journey.Models.Post
{
    public class PostActivity: BindableBase
    {
        private string action;
        public string Action
        {
            get => action;
            set => SetProperty(ref action, value);
        }

        private string activity;
        public string Activity
        {
            get => activity;
            set => SetProperty(ref activity, value);
        }


        private Media image;
        public Media Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }
    }
}

