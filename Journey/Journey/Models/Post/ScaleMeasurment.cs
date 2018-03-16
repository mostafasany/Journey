using Abstractions.Forms;
using Prism.Mvvm;

namespace Journey.Models.Post
{
    public class ScaleMeasurment : BindableBase
    {
        private Media image;

        private double measure;


        private string title;

        private string unit;
        public int Id { get; set; }

        public bool UpIndictor { get; set; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Unit
        {
            get => unit;
            set => SetProperty(ref unit, value);
        }

        public string MeasureWithUnit => string.Format("{0} {1}", Measure, unit);

        public double Measure
        {
            get => measure;
            set => SetProperty(ref measure, value);
        }

        public Media Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }


        public Media Indictor
        {
            get
            {
                string imagePath = UpIndictor ? "http://bit.ly/2zWMFQ7" : "http://bit.ly/2ySFsmK";
                return new Media {Path = imagePath};
            }
        }

        public string Color
        {
            get
            {
                string color = UpIndictor ? "#00ff00" : "#e55c43";
                return color;
            }
        }
    }
}