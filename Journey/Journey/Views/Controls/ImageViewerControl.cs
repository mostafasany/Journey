using Xamarin.Forms;
using System.Collections;

//https://github.com/rasmuschristensen/XamarinFormsImageGallery
namespace Journey.Views.Controls
{
    public class ImageViewerControl : ScrollView
    {
        readonly StackLayout _imageStack;
        public ImageViewerControl()
        {
            this.Orientation = ScrollOrientation.Horizontal;

            _imageStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };
            this.Content = _imageStack;
        }



        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<ImageViewerControl, IList>(
                view => view.ItemsSource,
                default(IList),
                BindingMode.TwoWay,
                propertyChanging: (bindableObject, oldValue, newValue) =>
                {
                    ((ImageViewerControl)bindableObject).ItemsSourceChanging();
                },
                propertyChanged: (bindableObject, oldValue, newValue) =>
                {
                    ((ImageViewerControl)bindableObject).ItemsSourceChanged(bindableObject, oldValue, newValue);
                }
            );

        public IList ItemsSource
        {
            get
            {
                return (IList)GetValue(ItemsSourceProperty);
            }
            set
            {

                SetValue(ItemsSourceProperty, value);
            }
        }

        void ItemsSourceChanging()
        {
            if (ItemsSource == null)
                return;
        }

        void ItemsSourceChanged(BindableObject bindable, IList oldValue, IList newValue)
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
                    var view = (View)ItemTemplate.CreateContent();
                    var bindableObject = view as BindableObject;
                    if (bindableObject != null)
                        bindableObject.BindingContext = newItem;
                    _imageStack.Children.Add(view);

                }
            }

        }



        public DataTemplate ItemTemplate
        {
            get;
            set;
        }

    }
}

