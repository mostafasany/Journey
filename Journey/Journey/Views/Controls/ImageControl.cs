using Xamarin.Forms;

namespace Journey.Views.Controls
{
    public class ImageControl : Image
    {
        public static readonly BindableProperty PortablePathProperty =
            BindableProperty.Create(nameof(PortablePath), typeof(string), typeof(ImageControl),null,
        BindingMode.TwoWay,
        null,
        (bindable, oldValue, newValue) =>
        {
            var val = newValue.ToString();
            (bindable as ImageControl).Source = ImageSource.FromResource(val);
        });

        public string PortablePath
        {
            get { return (string)GetValue(PortablePathProperty); }
            set
            { 
                SetValue(PortablePathProperty, value);
               
            }
        }
    }
}
