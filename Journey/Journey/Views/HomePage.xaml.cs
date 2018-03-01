using System;
using System.Linq;
using Journey.ViewModels;
using Journey.ViewModels.Wall;
using Journey.Views.Post;
using Unity;
using Xamarin.Forms;

namespace Journey.Views
{
    public partial class HomePage : BasePage
    {
        private HomePageViewModel _vm;

        public HomePage()
        {
            InitializeComponent();
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            SizeChanged += HomePage_SizeChanged;
        }

        private void HomePage_SizeChanged(object sender, EventArgs e)
        {
            var newPostWithHome = Width > 1000;
            if (newPostWithHome)
            {
                Header.IsVisible = false;
                Container.IsVisible = true;
            }
            else
            {
                Header.IsVisible = true;
                Container.IsVisible = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm = BindingContext as HomePageViewModel;
            var newPostWithHome = Width > 1000;
            if (newPostWithHome)
            {
                var newPostView = new NewPostView();
                var vm = BindingContext as HomePageViewModel;
                if (vm != null)
                {
                    newPostView.BindingContext = vm.Container.Resolve<NewPostPageViewModel>();
                    vm.Intialize(true);
                }

                Container.Content = newPostView;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (_vm.PostsViewModels?.FirstOrDefault() != null)
                lst.ScrollTo(_vm.PostsViewModels.FirstOrDefault(), ScrollToPosition.Center, false);
        }

        private void Handle_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var viewCellDetails = e.Item as PostBaseViewModel;
            var posts = _vm.PostsViewModels;
            var viewCellIndex = posts?.IndexOf(viewCellDetails);

            if (posts?.Count - 2 <= viewCellIndex)
                _vm.OnGetMorePostsCommand.Execute(null);
        }

        private void Handle_Focused(object sender, FocusEventArgs e)
        {
            _vm.OnSearchFriendsCommand.Execute(null);
        }
    }
}