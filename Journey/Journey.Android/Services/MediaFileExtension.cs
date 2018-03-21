using System;
using System.IO;
using System.Threading.Tasks;
using Abstractions.Forms;
using Android.Graphics;
using Android.Media;
using Journey.Droid.Services;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using MediaOrientation = Android.Media.Orientation;

[assembly: Dependency(typeof(MediaFileExtensions))]

namespace Journey.Droid.Services
{
    public class MediaFileExtensions : IMediaFileExtensions
    {
        /// <summary>
        ///     Rotate an image if required.
        /// </summary>
        /// <param name="file">The file image</param>
        /// <returns>True if rotation occured, else fal</returns>
        public async Task<bool> FixOrientationAsync(MediaFile file)
        {
            if (file == null)
                return false;
            try
            {
                string filePath = file.Path;
                int? orientation = GetRotation(filePath);

                if (!orientation.HasValue)
                    return false;

                Bitmap bmp = RotateImage(filePath, orientation.Value);
                var quality = 90;

                using (FileStream stream = File.Open(filePath, FileMode.OpenOrCreate))
                {
                    await bmp.CompressAsync(Bitmap.CompressFormat.Png, quality, stream);
                }

                bmp.Recycle();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static int? GetRotation(string filePath)
        {
            try
            {
                var ei = new ExifInterface(filePath);
                var orientation = (MediaOrientation) ei.GetAttributeInt(ExifInterface.TagOrientation, (int) MediaOrientation.Normal);
                switch (orientation)
                {
                    case MediaOrientation.Rotate90:
                        return 90;
                    case MediaOrientation.Rotate180:
                        return 180;
                    case MediaOrientation.Rotate270:
                        return 270;
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Bitmap RotateImage(string filePath, int rotation)
        {
            Bitmap originalImage = BitmapFactory.DecodeFile(filePath);

            var matrix = new Matrix();
            matrix.PostRotate(rotation);
            Bitmap rotatedImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);
            originalImage.Recycle();
            return rotatedImage;
        }
    }
}