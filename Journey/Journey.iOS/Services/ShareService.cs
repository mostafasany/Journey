using System.Collections.Generic;
using System.Linq;
using Foundation;
using Journey.iOS.Services;
using Journey.Services.Forms;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(ShareService))]
namespace Journey.iOS.Services
{
    public class ShareService:IShare
    {
        public async void Share(string subject, string message,List<ImageSource> image)
        {
            var handler = new ImageLoaderSourceHandler();
            var uiImage = await handler.LoadImageAsync(image.FirstOrDefault());

            var img = NSObject.FromObject(uiImage);
            var mess = NSObject.FromObject(message);

            var activityItems = new[] { mess, img };
            var activityController = new UIActivityViewController(activityItems, null);

            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }

            topController.PresentViewController(activityController, true, () => { });
        }
    }
}
