using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Abstractions.Forms;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Java.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using File = Java.IO.File;
using ShareService = Journey.Droid.Services.ShareService;

[assembly: Dependency(typeof(ShareService))]

namespace Journey.Droid.Services
{
    public class ShareService : Activity, IShare
    {
        public async Task Share(string subject, string message,
            List<Media> mediaItems)
        {
            var intent = new Intent(Intent.ActionSendMultiple);
            intent.PutExtra(Intent.ExtraSubject, subject);
            intent.PutExtra(Intent.ExtraText, message);
            intent.SetType("image/*");
            intent.SetType("video/*");

            var handler = new ImageLoaderSourceHandler();


            var files = new List<Uri>();
            var id = 0;
            foreach (Media media in mediaItems)
                if (media.Type == MediaType.Image)
                {
                    Bitmap bitmap = await handler.LoadImageAsync(media.Source, this);
                    string ex = media.Ext;
                    string fileName = $"{id++}{ex}";
                    File path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads
                                                                              + File.Separator + fileName);

                    using (var os = new FileStream(path.AbsolutePath, FileMode.Create))
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
                    }

                    files.Add(Uri.FromFile(path));
                }
                else
                {
                    string ex = media.Ext;
                    string fileName = $"{id++}{ex}";
                    File path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads
                                                                              + File.Separator + fileName);
                    var filestream = new FileOutputStream(path);
                    filestream.Write(media.SourceArray);
                    filestream.Close();
                }

            var uris = new List<IParcelable>();
            files.ForEach(file =>
            {
                uris.Add(file);
            });
            //TODO: Should Clear images after share
            intent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
            Forms.Context.StartActivity(Intent.CreateChooser(intent, "Share Image"));
        }

        public async Task Share(string subject, string message, byte[] video)
        {
            var filestream = new FileOutputStream("test.mp4");
            filestream.Write(video);
            filestream.Close();
        }
    }
}