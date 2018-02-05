using System;

namespace Abstractions.Models
{
    public class Location
    {
        public string Id { get; set; }

        public double Near { get; set; }

        public string NearFormated => string.Format("Near by {0} KM", Math.Round(Near, 2));

        public string Name { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public Media Media { get; set; }
    }
}