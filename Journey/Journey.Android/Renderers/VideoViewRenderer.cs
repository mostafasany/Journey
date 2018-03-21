using System;
using Journey.Droid.Renderers;
using Journey.Views.Controls;
using Plugin.MediaManager;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(VideoView), typeof(VideoViewRenderer))]

namespace Journey.Droid.Renderers
{
    public class VideoViewRenderer : ViewRenderer<VideoView, VideoSurface>
    {
        private VideoSurface _videoSurface;

        public static void Init()
        {
            DateTime temp = DateTime.Now;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<VideoView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                _videoSurface = new VideoSurface(Context);
                SetNativeControl(_videoSurface);
                CrossMediaManager.Current.VideoPlayer.RenderSurface = _videoSurface;
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            LayoutParams p = _videoSurface.LayoutParameters;
            p.Height = heightMeasureSpec;
            p.Width = widthMeasureSpec;
            _videoSurface.LayoutParameters = p;
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }
    }
}