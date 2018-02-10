﻿using System;
using System.Linq;
using Journey.ViewModels;
using Journey.ViewModels.Wall;
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
            img.GestureRecognizers.Add(tapGestureRecognizer);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm = BindingContext as HomePageViewModel;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (_vm.PostsViewModels != null && _vm.PostsViewModels.FirstOrDefault() != null)
                lst.ScrollTo(_vm.PostsViewModels.FirstOrDefault(), ScrollToPosition.Center, false);
        }

        private void Handle_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var viewCellDetails = e.Item as PostBaseViewModel;
            var posts = _vm.PostsViewModels;
            if (posts == null)
                return;
            var viewCellIndex = posts.IndexOf(viewCellDetails);

            if (posts.Count - 2 <= viewCellIndex)
                _vm.OnGetMorePostsCommand.Execute(null);
        }

        private void Handle_Focused(object sender, FocusEventArgs e)
        {
            _vm.OnSearchFriendsCommand.Execute(null);
        }
    }
}