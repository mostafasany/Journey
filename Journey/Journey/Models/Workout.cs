using System.Collections.Generic;
using Prism.Mvvm;

namespace Journey.Models
{
    public class Workout : BindableBase
    {
        private string image;
        private string maxRips;
        private string maxWeight;

        private string name;
        private string rips;

        private string unit = "Kg";
        private string weight;
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

        public string MaxWeight
        {
            get 
            {
                return string.IsNullOrEmpty(maxWeight)?"Max Weight":maxWeight; 
            }
            set => SetProperty(ref maxWeight, value);
        }


        public string MaxRips
        {
            get
            {
                return string.IsNullOrEmpty(maxRips) ? "Max Rips" : maxRips;
            }
            set => SetProperty(ref maxRips, value);
        }
    }
}