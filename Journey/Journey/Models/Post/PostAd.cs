namespace Journey.Models.Post
{
    public class PostAd : PostBase
    {
        private Media image;

        public Media Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public string DeepLink { get; set; }
    }
}