using System.Collections.Generic;
using Prism.Mvvm;

namespace Journey.Models
{
    public class Workout : BindableBase
    {
        private string image;

        private string name;

        private string unit = "Kg";
        private string weight;
        private string rips;
        private List<Workout> workouts;
        public string Id { get; set; }
        public string Parent { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
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

        public string Rips
        {
            get => rips;
            set => SetProperty(ref rips, value);
        }

        public string Unit
        {
            get => unit;
            set => SetProperty(ref unit, value);
        }

        public List<Workout> Workouts
        {
            get => workouts;
            set => SetProperty(ref workouts, value);
        }

    }
}