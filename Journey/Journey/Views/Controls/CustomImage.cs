using System;
using FFImageLoading.Forms;

namespace Journey.Views.Controls
{
    public class CustomImage : CachedImage
    {
        public CustomImage()
        {
            DownsampleToViewSize = true;
            // LoadingPlaceholder = "icon.png";
            // ErrorPlaceholder = "icon.png";
            CacheDuration = TimeSpan.FromDays(30);

            //RetryCount = 0;
            //RetryDelay = 250;
            //TransparencyEnabled = false;
        }
    }
}