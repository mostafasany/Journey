//using System;
//using System.Collections.Generic;
//using Android.App;
//using Android.Content;
//using Android.Provider;
//using Android.Widget;
//using Java.IO;
//using Xamarin.Forms;
//using Abstractions.Services.Contracts;
//using Abstractions.Models;
//using Journey.Constants;
//namespace Journey.Droid.Services
//{
//    public class MediaService : Java.Lang.Object, IMediaService
//    {
//        private static readonly Int32 REQUEST_CAMERA = 0;
//        private static readonly Int32 SELECT_FILE = 1;
//        internal const string ExtraSaveToAlbum = "album_save";
//        public event ImageChangedEventHandler ImageChangedEventHandler;

//        public void ImageChanged(List<Media> Images)
//        {
//            ImageChangedEventHandler?.Invoke(this, new ImageChangedArgs { Images = Images });
//        }

//        public void OpenCamera()
//        {
//            var context = ((Activity)Forms.Context);
//            var intent = new Intent(MediaStore.ActionImageCapture);
//            //App._file = new File((File)App._dir, String.Format("{0}.{1}", Guid.NewGuid(), Constant.DefaultImageExt));
//            //intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile((File)App._file));
//            context.StartActivityForResult(intent, REQUEST_CAMERA);
//        }

//        public void OpenGallery()
//        {

//            var context = ((Activity)Forms.Context);

//            Toast.MakeText(Forms.Context, "Select max 15 media item", ToastLength.Long).Show();

//            var imageIntent = new Intent(Intent.ActionPick);
//            imageIntent.SetType("image/* video/*");
//            imageIntent.PutExtra(Intent.ExtraAllowMultiple, true);
//            imageIntent.PutExtra(Intent.ExtraMimeTypes, new String[] { "image/*", "video/*" });
//            imageIntent.SetAction(Intent.ActionGetContent);

//            context.StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), SELECT_FILE);
//        }

//        public void PickAsync()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}