using System;
using System.IO;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Plugin.Media;
using Plugin.Media.Abstractions;

//https://github.com/jamesmontemagno/MediaPlugin
namespace Abstractions.Forms
{
    public class MediaService : IMediaService<Media>
    {
        private readonly CameraDevice DefaultCamera = CameraDevice.Front;
        private readonly PhotoSize PhotoSize = PhotoSize.Small;
        private readonly VideoQuality VideoQuality = VideoQuality.Low;
        private const string VideoPlaceHolderPath = "http://bit.ly/2EiCAic";
        private const int CompressionQuality = 30;
        private const int CustomPhotoSize = 10;
        private const bool SaveToAlbum = true;


        public async Task<Media> PickPhotoAsync()
        {
            try
            {
                MediaFile media = await CrossMedia.Current.PickPhotoAsync(
                    new PickMediaOptions
                    {
                        PhotoSize = PhotoSize,
                        CustomPhotoSize = CustomPhotoSize
                    });
                if (media == null)
                    return null;
                Stream stream = media.GetStream();
                byte[] array = ReadFully(stream);
                var image = new Media
                {
                    OriginalName = Path.GetFileName(media.Path),
                    Name = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(media.Path)),
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
                MediaFile media = await CrossMedia.Current.PickVideoAsync();
                if (media == null)
                    return null;
                Stream stream = media.GetStream();
                byte[] array = ReadFully(stream);
                var image = new Media
                {
                    OriginalName = Path.GetFileName(media.Path),
                    Name = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(media.Path)),
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
                MediaFile media = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        CompressionQuality = CompressionQuality,
                        PhotoSize = PhotoSize,
                        CustomPhotoSize = CustomPhotoSize,
                        SaveToAlbum = SaveToAlbum,
                        DefaultCamera = DefaultCamera,
                        RotateImage = false
                    }
                );

                //  await DependencyService.Get<IMediaFileExtensions>().FixOrientationAsync(media).ConfigureAwait(false);

                if (media == null)
                    return null;
                Stream stream = media.GetStream();
                byte[] array = ReadFully(stream);
                var image = new Media
                {
                    OriginalName = Path.GetFileName(media.Path),
                    Name = Path.GetFileName(media.Path),
                    Path = media.Path,
                    Thumbnail = media.Path,
                    SourceArray = array,
                    Ext = Path.GetExtension(media.Path)
                };
                return image;
            }
            catch (NotSupportedException e)
            {
                throw e;
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
                MediaFile media = await CrossMedia.Current.TakeVideoAsync(
                    new StoreVideoOptions
                    {
                        Quality = VideoQuality,
                        CompressionQuality = CompressionQuality,
                        PhotoSize = PhotoSize,
                        SaveToAlbum = SaveToAlbum,
                        DefaultCamera = DefaultCamera,
                        RotateImage = false
                    });

                if (media == null)
                    return null;
                Stream stream = media.GetStream();
                byte[] array = ReadFully(stream);
                var image = new Media
                {
                    OriginalName = Path.GetFileName(media.Path),
                    Name = Path.GetFileName(media.Path),
                    Path = media.Path,
                    Thumbnail = VideoPlaceHolderPath,
                    SourceArray = array,
                    Ext = Path.GetExtension(media.Path),
                    Type = MediaType.Video
                };
                return image;
            }

            catch (NotSupportedException e)
            {
                throw e;
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