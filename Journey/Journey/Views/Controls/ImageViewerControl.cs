using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

//https://github.com/rasmuschristensen/XamarinFormsImageGallery
namespace Journey.Views.Controls
{
    public class ImageViewerControl : ScrollView
    {
        private readonly StackLayout _imageStack;

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

        public bool EqualsAll<T>(ObservableCollection<T> a, ObservableCollection<T> b)
        {
            if (a == null || b == null)
                return a == null && b == null;

            if (a.Count != b.Count)
                return false;

            return a.SequenceEqual(b);
        }

        private void ItemsSourceChanged(BindableObject bindable, IList oldValue, IList newValue)
        {
            if (ItemsSource == null)
                return;

            //var notifyCollection = newValue as INotifyCollectionChanged;
            //if (notifyCollection != null)
            //{

            //ObservableCollection<Abstractions.Forms.Media> oldList=oldValue as ObservableCollection<Abstractions.Forms.Media>;
            //ObservableCollection<Abstractions.Forms.Media> newList = newValue as ObservableCollection<Abstractions.Forms.Media>;
            //var areEqual= EqualsAll<Abstractions.Forms.Media>(oldList, newList);
            //if (areEqual)
            //{
            //    return;
            //}

            if (newValue != null)
            {
                _imageStack.Children.Clear();
                // int i = 0;
                foreach (object newItem in newValue)
                {
                    //i++;
                    //if (i == 4)
                    //break;

                    var view = (ViewCell) ItemTemplate.CreateContent();
                    //var view = (View) ItemTemplate.CreateContent();
                    var bindableObject = view.View as BindableObject;
                    if (bindableObject != null)
                        bindableObject.BindingContext = newItem;
                    _imageStack.Children.Add(view.View);
                }
            }
        }

        private void ItemsSourceChanging()
        {
            if (ItemsSource == null)
                return;
        }
    }
}