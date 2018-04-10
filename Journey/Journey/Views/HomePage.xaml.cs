using System;
using System.Collections.ObjectModel;
using Journey.ViewModels;
using Journey.ViewModels.Wall;
using Journey.Views.Post;
using Xamarin.Forms;

namespace Journey.Views
{
    public partial class HomePage : BasePage
    {
        private HomePageViewModel _vm;

        public HomePage()
        {
            InitializeComponent();
            //var tapGestureRecognizer = new TapGestureRecognizer();
            //tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            SizeChanged += HomePage_SizeChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm = BindingContext as HomePageViewModel;
        }

        private void Handle_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var viewCellDetails = e.Item as PostBaseViewModel;
            ObservableCollection<PostBaseViewModel> posts = _vm.PostsViewModels;
            int? viewCellIndex = posts?.IndexOf(viewCellDetails);

            if (posts?.Count - 2 <= viewCellIndex)
                _vm.OnGetMorePostsCommand.Execute(null);
        }

        private void HomePage_SizeChanged(object sender, EventArgs e)
        {
            bool newPostWithHome = Width > 1000;
            if (newPostWithHome)
            {
                Header.IsVisible = false;
                Container.IsVisible = true;
                if (Container.Content == null)
                {
                    var newPostView = new NewPostView();
                    var vm = BindingContext as HomePageViewModel;
                    if (vm != null)
                    {
                        NewPostPageViewModel postVm = vm.NewPostPageViewModel;
                        newPostView.BindingContext = postVm;
                        postVm.Intialize(true);
                    }

                    Container.Content = newPostView;
                }
            }
            else
            {
                Header.IsVisible = true;
                Container.IsVisible = false;
            }
        }

        //private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        //{
        //    if (_vm.PostsViewModels?.FirstOrDefault() != null)
        //        PostList.ScrollTo(_vm.PostsViewModels.FirstOrDefault(), ScrollToPosition.Center, false);
        //}
    }
}