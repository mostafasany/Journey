using System.Threading.Tasks;
using Journey.iOS.Services;
using Journey.Services.Forms;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaFileExtensions))]
namespace Journey.iOS.Services
{
    public class MediaFileExtensions : IMediaFileExtensions
    {
        public async  Task<bool> FixOrientationAsync(MediaFile file)
        {
            return true;
        }
    }
}
