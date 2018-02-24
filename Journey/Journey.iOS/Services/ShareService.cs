using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Abstractions.Forms;
[assembly: Dependency(typeof(Journey.iOS.Services.ShareService))]
namespace Journey.iOS.Services
{
    public class ShareService:IShare
    {
        public async Task Share(string subject, string message,List<Media> mediaItems)
        {
            var handler = new ImageLoaderSourceHandler();
            List<NSObject> activityItems=new List<NSObject>();
            activityItems.Add(NSObject.FromObject(message));
            foreach (var media in mediaItems)
            {
                var uiImage = await handler.LoadImageAsync(media.Source);
                var img = NSObject.FromObject(uiImage);
                activityItems.Add(NSObject.FromObject(img));
            }
          
            var activityController = new UIActivityViewController(activityItems.ToArray(), null);

            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }

            topController.PresentViewController(activityController, true, () => { });
        }
    }
}
