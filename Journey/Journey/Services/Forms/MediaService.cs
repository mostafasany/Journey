using System;
using System.IO;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Journey.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace Journey.Services.Forms
{
    internal class MediaService : IMediaService<Media>
    {
        public async Task<Media> PickPhotoAsync()
        {
            try
            {
                var media = await CrossMedia.Current.PickPhotoAsync(
                    new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium
                    });
                var stream = media.GetStream();
                var array = ReadFully(stream);
                var image = new Media
                {
                    Path = media.Path,
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
                var stream = media.GetStream();
                var array = ReadFully(stream);
                var image = new Media
                {
                    Path = media.Path,
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

        public async Task<Media> TakePhotoAsync()
        {
            try
            {
                var media = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions {AllowCropping = true});
                var stream = media.GetStream();
                var array = ReadFully(stream);
                var image = new Media
                {
                    Path = media.Path,
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
                var media = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions());
                var stream = media.GetStream();
                var array = ReadFully(stream);
                var image = new Media
                {
                    Path = media.Path,
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