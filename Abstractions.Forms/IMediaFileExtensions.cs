using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace Journey.Services.Forms
{
    public interface IMediaFileExtensions
    {
        /// <summary>
        ///     Does nothing
        /// </summary>
        /// <param name="file">The file image</param>
        /// <returns>True if rotation occured, else false</returns>
        Task<bool> FixOrientationAsync(MediaFile file);
    }
}