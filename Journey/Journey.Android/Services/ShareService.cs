using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Java.IO;
using Journey.Droid.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(ShareService))]
namespace Journey.Droid.Services
{
    public class ShareService : Activity, Journey.Services.Forms.IShare
    {
        public async Task Share(string subject, string message,
                                List<Models.Media> mediaItems)
        {
            var intent = new Intent(Intent.ActionSendMultiple);
            intent.PutExtra(Intent.ExtraSubject, subject);
            intent.PutExtra(Intent.ExtraText, message);
            intent.SetType("image/*");
            intent.SetType("video/*");   
            var handler = new ImageLoaderSourceHandler();


            List<Android.Net.Uri> files = new List<Android.Net.Uri>();
            int id = 0;
            foreach (var media in mediaItems)
            {
                if(media.Type==Models.MediaType.Image)
                {
                    var bitmap = await handler.LoadImageAsync(media.Source, this);
                    var ex = media.Ext;
                    var fileName = string.Format("{0}{1}", id++, ex);
                    var path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads
                                                                             + Java.IO.File.Separator + fileName);

                    using (var os = new System.IO.FileStream(path.AbsolutePath, System.IO.FileMode.Create))
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
                    }
                    files.Add(Android.Net.Uri.FromFile(path));
                }
                else
                {
                    var ex = media.Ext;
                    var fileName = string.Format("{0}{1}", id++, ex);
                    var path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads
                                                                             + Java.IO.File.Separator + fileName);
                                          FileOutputStream filestream = new FileOutputStream(path);
                    filestream.Write(media.SourceArray);
                    filestream.Close();
                }


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
            FileOutputStream filestream = new FileOutputStream("test.mp4");
            filestream.Write(video);
            filestream.Close();

        }
    }


}