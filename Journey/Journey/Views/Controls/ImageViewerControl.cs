using System.Collections;
using Xamarin.Forms;

//https://github.com/rasmuschristensen/XamarinFormsImageGallery
namespace Journey.Views.Controls
{
    public class ImageViewerControl : ScrollView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<ImageViewerControl, IList>(
                view => view.ItemsSource,
                default(IList),
                BindingMode.TwoWay,
                propertyChanging: (bindableObject, oldValue, newValue) =>
                {
                    ((ImageViewerControl) bindableObject).ItemsSourceChanging();
                },
                propertyChanged: (bindableObject, oldValue, newValue) =>
                {
                    ((ImageViewerControl) bindableObject).ItemsSourceChanged(bindableObject, oldValue, newValue);
                }
            );

        private readonly StackLayout _imageStack;

        public ImageViewerControl()
        {
            Orientation = ScrollOrientation.Horizontal;

            _imageStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };
            Content = _imageStack;
        }

        public IList ItemsSource
        {
            get => (IList) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }


        public DataTemplate ItemTemplate { get; set; }

        private void ItemsSourceChanging()
        {
            if (ItemsSource == null)
                return;
        }

        private void ItemsSourceChanged(BindableObject bindable, IList oldValue, IList newValue)
        {
            if (ItemsSource == null)
                return;

            //var notifyCollection = newValue as INotifyCollectionChanged;
            //if (notifyCollection != null)
            //{
            if (newValue != null)
            {
                _imageStack.Children.Clear();
                // int i = 0;
                foreach (var newItem in newValue)
                {
                    //i++;
                    //if (i == 4)
                    //break;
                    var view = (View) ItemTemplate.CreateContent();
                    var bindableObject = view as BindableObject;
                    if (bindableObject != null)
                        bindableObject.BindingContext = newItem;
                    _imageStack.Children.Add(view);
                }
            }
        }
    }
}