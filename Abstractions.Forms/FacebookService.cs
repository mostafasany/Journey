using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Contracts;
using Abstractions.Exceptions;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Unity;

namespace Abstractions.Forms
{
    public class FacebookService : BaseService, IFacebookService
    {
        private readonly ILocationService _locationService;

        private const string ApiMe = "https://graph.facebook.com/me?fields=id,name,picture.type(large)";

        private const string ApiSearchWithDistance =
            "https://graph.facebook.com/search?q={0}&distance={1}&type={2}&center={3},{4}&fields=id,name,talking_about_count,checkins,location,picture.type(large)";

        private const string ApiSearch =
           "https://graph.facebook.com/search?q={0}&type={1}&center={2},{3}&fields=id,name,talking_about_count,checkins,location,picture.type(large)";


        public FacebookService(IUnityContainer container, IHttpService httpService, ILocationService locationService,
            IExceptionService exceptionService) : base(container)
        {
            HttpService = httpService;
            _locationService = locationService;
            ExceptionService = exceptionService;
        }

        public async Task<List<Location>> GetLocationsAsync(string name, double lat, double lng, int? minLocationDistanceInMeter)
        {
            try
            {
                if (string.IsNullOrEmpty(FacebookToken))
                    throw new ArgumentException(nameof(FacebookToken));
                var locations = new List<Location>();

                string api = "";
                if (minLocationDistanceInMeter == null)
                    api = string.Format(ApiSearch, name, "place", lat, lng);
                else
                    api = string.Format(ApiSearchWithDistance, name, minLocationDistanceInMeter, "place", lat, lng);

                api += " &access_token=" + FacebookToken;
                HttpResult<FacebookLocationRoot> result = await HttpService.HttpGetAsync<FacebookLocationRoot>(api);
                FacebookLocationRoot data = result.Result;
                foreach (FacebookLocation location in data.data)
                {
                    var fr = new Location
                    {
                        Id = location.id,
                        Lat = location.location.latitude,
                        Lng = location.location.longitude,
                        Name = location.name,
                        Near = _locationService.DistanceBetweenPlaces(lng, lat, location.location.longitude,
                            location.location.latitude),
                        Image = location.picture?.data?.url
                    };
                    locations.Add(fr);
                }
                if (minLocationDistanceInMeter != null)
                {
                    double minLocation= minLocationDistanceInMeter.Value / 1000.0;
                    return locations.Where(loc => loc.Near <= minLocation)
                      .OrderBy(loc => loc.Near).ToList();
                }
                  
                else return locations;
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex.Message);
            }
        }

        public string FacebookToken { get; set; }

        public string FacebookTokenKey { get; } = "FacebookToken";

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