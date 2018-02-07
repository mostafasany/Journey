using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Contracts;
using Abstractions.Exceptions;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Unity;

namespace Journey.Services.Forms
{
    internal class FacebookService : BaseService, IFacebookService
    {
        private const int MinLocationDistanceInMeter = 500;
        private const double MinLocationDistanceInKm = 0.5;

        private const string ApiMe = "https://graph.facebook.com/me?fields=id,name,picture.type(large)";

        private const string ApiSearch =
                "https://graph.facebook.com/search?q={0}&distance={1}&type={2}&center={3},{4}&fields=id,name,talking_about_count,checkins,location,picture.type(large)"
            ;

        private readonly ILocationService _locationService;

        public FacebookService(IUnityContainer container, IHttpService httpService, ILocationService locationService,
            IExceptionService exceptionService) : base(container)
        {
            HttpService = httpService;
            _locationService = locationService;
            ExceptionService = exceptionService;
        }

        public async Task<List<Location>> GetLocationsAsync(string name, double lat, double lng)
        {
            try
            {
                if (string.IsNullOrEmpty(FacebookToken))
                    throw new ArgumentException(nameof(FacebookToken));
                var locations = new List<Location>();

                //var facebookAccount = facebookservice.GetFacebookAccount();
                var api = string.Format(ApiSearch, name, MinLocationDistanceInMeter, "place", lat,
                    lng);
                api += " &access_token=" + FacebookToken;
                var result = await HttpService.HttpGetAsync<FacebookLocationRoot>(api);
                var data = result.Result;
                foreach (var location in data.data)
                {
                    var fr = new Location
                    {
                        Id = location.id,
                        Lat = location.location.latitude,
                        Lng = location.location.longitude,
                        Name = location.name,
                        Near = _locationService.DistanceBetweenPlaces(lng, lat, location.location.longitude,
                            location.location.latitude),
                        Image  = location.picture?.data?.url
                    };
                    locations.Add(fr);
                }

                return locations.Where(loc => loc.Near <= MinLocationDistanceInKm)
                    .OrderBy(loc => loc.Near).ToList();
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }

        public string FacebookToken { get; set; }

        public void InviteFriends()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }

        public void Share(string urlImage, string message, string name, string link, string description)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }
    }
}