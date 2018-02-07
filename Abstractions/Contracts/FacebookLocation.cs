using System.Collections.Generic;

namespace Abstractions.Contracts
{
    //id,name,talking_about_count,checkins,location,picture.type(large)
    public class FacebookLocation
    {
        public string id { get; set; }

        public string name { get; set; }

        public int talking_about_count { get; set; }

        public int checkins { get; set; }

        public FacebookLocationObj location { get; set; }

        public FacebookPictureRoot picture { get; set; }
    }

    //"location": {
    //   "city": "San Francisco",
    //   "country": "United States",
    //   "latitude": 37.784916242722,
    //   "longitude": -122.40201715675,
    //   "state": "CA",
    //   "street": "701 Mission St",
    //   "zip": "94103"
    //},
    public class FacebookLocationObj
    {
        public string city { get; set; }

        public string country { get; set; }

        public string state { get; set; }

        public string street { get; set; }

        public string zip { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }
    }

    public class FacebookLocationRoot
    {
        public List<FacebookLocation> data { get; set; }
    }
}