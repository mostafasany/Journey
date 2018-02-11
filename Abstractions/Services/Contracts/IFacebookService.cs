using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Models;

namespace Abstractions.Services.Contracts
{
    public interface IFacebookService
    {
        //https://forums.xamarin.com/discussion/48944/has-anyone-had-success-using-xamarin-facebook-appinvitedialog
        //https://stackoverflow.com/questions/17107933/facebook-app-request-not-sending-to-facebook-friends-through-facebook-android-sd
        //https://developers.facebook.com/quickstarts/1066414026732641/?platform=app-links-host
        string FacebookTokenKey { get; }

        string FacebookToken { get; set; }

        void InviteFriends();

        Task<List<Location>> GetLocationsAsync(string name, double lat, double lng);

        void Share(string urlImage, string message, string name, string link, string description);
    }
}