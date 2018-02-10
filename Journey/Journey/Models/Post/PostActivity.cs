using Prism.Mvvm;

namespace Journey.Models.Post
{
    public class PostActivity : BindableBase
    {
        private string action;

        private string activity;


        private Media image;

        public string Action
        {
            get => action;
            set => SetProperty(ref action, value);
        }

        public string Activity
        {
            get => activity;
            set => SetProperty(ref activity, value);
        }

        public Media Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }
    }
}