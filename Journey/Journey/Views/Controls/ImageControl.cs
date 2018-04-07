using System;
using Xamarin.Forms;

namespace Journey.Views.Controls
{
    public class ImageControl : Image
    {
        public static readonly BindableProperty PortablePathProperty =
            BindableProperty.Create(nameof(PortablePath), typeof(string), typeof(ImageControl), null,
                BindingMode.TwoWay,
                null,
                (bindable, oldValue, newValue) =>
                {
                    string val = newValue.ToString();
                    try
                    {
                        var source = Device.RuntimePlatform == Device.UWP
                            ? ImageSource.FromFile("Assets/" + val)
                            : ImageSource.FromResource("Journey.Assets." + val);
                        ((ImageControl) bindable).Source = source;
                    }
                    catch (Exception)
                    {
                    }
                });

        public string PortablePath
        {
            get => (string)GetValue(PortablePathProperty);
            set => SetValue(PortablePathProperty, value);
        }
    }

    public class SVGImageControl : FFImageLoading.Svg.Forms.SvgCachedImage
    {
        public static readonly BindableProperty PortablePathProperty =
            BindableProperty.Create(nameof(PortablePath), typeof(string), typeof(ImageControl), null,
                BindingMode.TwoWay,
                null,
                (bindable, oldValue, newValue) =>
                {
                    string val = newValue.ToString();
                    try
                    {
                        var source= Device.RuntimePlatform == Device.UWP
                            ? FFImageLoading.Svg.Forms.SvgImageSource.FromFile("Assets/" + val)
                              //  ? FFImageLoading.Svg.Forms.SvgImageSource.FromUri(new Uri("https://s.cdpn.io/3/kiwi.svg"))
                            : FFImageLoading.Svg.Forms.SvgImageSource.FromResource("Journey.Assets.SVG." + val);

                        ((SVGImageControl) bindable).Source = source;

                    }
                    catch (Exception ex)
                    {
                    }
                });

        public string PortablePath
        {
            get => (string)GetValue(PortablePathProperty);
            set => SetValue(PortablePathProperty, value);
        }
    }
}