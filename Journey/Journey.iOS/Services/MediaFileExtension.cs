using System.Threading.Tasks;
using Abstractions.Forms;
using Journey.iOS.Services;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaFileExtensions))]

namespace Journey.iOS.Services
{
    public class MediaFileExtensions : IMediaFileExtensions
    {
        public async Task<bool> FixOrientationAsync(MediaFile file)
        {
            return true;
        }
    }
}