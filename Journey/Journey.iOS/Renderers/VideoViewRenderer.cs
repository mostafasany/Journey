using System;
using Foundation;
using Journey.iOS.Renderers;
using Journey.Views.Controls;
using Plugin.MediaManager;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(VideoView), typeof(VideoViewRenderer))]

namespace Journey.iOS.Renderers
{
    [Preserve(AllMembers = true)]
    public class VideoViewRenderer : ViewRenderer<VideoView, VideoSurface>
    {
        private VideoSurface _videoSurface;

        /// <summary>
        ///     Used for registration with dependency service
        /// </summary>
        public static async void Init()
        {
            DateTime temp = DateTime.Now;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<VideoView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                _videoSurface = new VideoSurface();
                SetNativeControl(_videoSurface);
                CrossMediaManager.Current.VideoPlayer.RenderSurface = _videoSurface;
            }
        }
    }
}