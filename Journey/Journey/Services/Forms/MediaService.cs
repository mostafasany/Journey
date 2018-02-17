using System;
using System.IO;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Journey.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;

//https://github.com/jamesmontemagno/MediaPlugin
namespace Journey.Services.Forms
{
    internal class MediaService : IMediaService<Media>
    {
        private const string VideoPlaceHolderPath = "http://bit.ly/2EiCAic";
        private const int CompressionQuality = 50;
        private readonly PhotoSize PhotoSize = PhotoSize.Small;
        private readonly VideoQuality VideoQuality = VideoQuality.Low;

        public async Task<Media> PickPhotoAsync()
        {
            try
            {
                var media = await CrossMedia.Current.PickPhotoAsync(
                    new PickMediaOptions
                    {
                        PhotoSize = PhotoSize
                    });
                if (media == null)
                    return null;
                var stream = media.GetStream();
                var array = ReadFully(stream);
                var image = new Media
                {
                    Path = media.Path,
                    Thumbnail = media.Path,
                    SourceArray = array,
                    Ext = Path.GetExtension(media.Path)
                };
                return image;
            }
            catch (Exception e)
            {
                throw new CoreServiceException(e.Message, e);
            }
        }

        public async Task<Media> PickVideoAsync()
        {
            try
            {
                var media = await CrossMedia.Current.PickVideoAsync();
                if (media == null)
                    return null;
                var stream = media.GetStream();
                var array = ReadFully(stream);
                var image = new Media
                {
                    Path = media.Path,
                    Thumbnail = VideoPlaceHolderPath,
                    SourceArray = array,
                    Ext = Path.GetExtension(media.Path),
                    Type = MediaType.Video
                };
                return image;
            }
            catch (Exception e)
            {
                throw new CoreServiceException(e.Message, e);
            }
        }

        public async Task<Media> TakePhotoAsync()
        {
            try
            {
                var media = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions {AllowCropping = true, CompressionQuality = CompressionQuality});
                if (media == null)
                    return null;
                var stream = media.GetStream();
                var array = ReadFully(stream);
                var image = new Media
                {
                    Path = media.Path,
                    Thumbnail = media.Path,
                    SourceArray = array,
                    Ext = Path.GetExtension(media.Path)
                };
                return image;
            }
            catch (Exception e)
            {
                throw new CoreServiceException(e.Message, e);
            }
        }

        public async Task<Media> TakeVideoAsync()
        {
            try
            {
                var media = await CrossMedia.Current.TakeVideoAsync(
                    new StoreVideoOptions {Quality = VideoQuality, CompressionQuality = CompressionQuality});
                if (media == null)
                    return null;
                var stream = media.GetStream();
                var array = ReadFully(stream);
                var image = new Media
                {
                    Path = media.Path,
                    Thumbnail = VideoPlaceHolderPath,
                    SourceArray = array,
                    Ext = Path.GetExtension(media.Path),
                    Type = MediaType.Video
                };
                return image;
            }
            catch (Exception e)
            {
                throw new CoreServiceException(e.Message, e);
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }
    }
}