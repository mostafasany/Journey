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
                        (bindable as ImageControl).Source = Device.RuntimePlatform == Device.UWP
                            ? ImageSource.FromFile("Assets/" + val)
                            : ImageSource.FromResource("Journey.Assets." + val);
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
                        var s1 = FFImageLoading.Svg.Forms.SvgImageSource.FromResource("Journey.Assets." + val);
                        var s = FFImageLoading.Svg.Forms.SvgImageSource.FromFile("Journey.Assets." + val);
                        (bindable as SVGImageControl).Source = s1;

                    }
                    catch (Exception ex)
                    {
                        var s = ex.Message;
                    }
                });

        public string PortablePath
        {
            get => (string)GetValue(PortablePathProperty);
            set => SetValue(PortablePathProperty, value);
        }
    }
}