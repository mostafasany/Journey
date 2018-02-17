using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Journey.Droid.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(ShareService))]
namespace Journey.Droid.Services
{
    public class ShareService : Activity, Journey.Services.Forms.IShare
    {
        public async void Share(string subject, string message,
                                List<ImageSource> image)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.PutExtra(Intent.ExtraSubject, subject);
            intent.PutExtra(Intent.ExtraText, message);
            intent.SetType("image/png");

            var handler = new ImageLoaderSourceHandler();
            var bitmap = await handler.LoadImageAsync(image.FirstOrDefault(), this);

            var path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads
                + Java.IO.File.Separator + "logo.png");

            using (var os = new System.IO.FileStream(path.AbsolutePath, System.IO.FileMode.Create))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
            }

            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(path));
            //intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(path));
            Forms.Context.StartActivity(Intent.CreateChooser(intent, "Share Image"));
        }
    }
}