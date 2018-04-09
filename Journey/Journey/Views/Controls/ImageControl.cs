using System;
using FFImageLoading.Svg.Forms;
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
                        ImageSource source = Device.RuntimePlatform == Device.UWP
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
            get => (string) GetValue(PortablePathProperty);
            set => SetValue(PortablePathProperty, value);
        }
    }

    public class SVGImageControl : SvgCachedImage
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
                        SvgImageSource source = Device.RuntimePlatform == Device.UWP
                            ? SvgImageSource.FromFile("Assets/SVG/" + val)
                            : SvgImageSource.FromResource("Journey.Assets.SVG." + val);

                        ((SVGImageControl) bindable).Source = source;
                    }
                    catch (Exception ex)
                    {
                    }
                });

        public string PortablePath
        {
            get => (string) GetValue(PortablePathProperty);
            set => SetValue(PortablePathProperty, value);
        }
    }
}