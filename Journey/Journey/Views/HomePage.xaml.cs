using Journey.ViewModels;

namespace Journey.Views
{
    public partial class HomePage : BasePage
    {
        readonly HomePageViewModel vm;
        public HomePage()
        {
            InitializeComponent();
            vm = BindingContext as HomePageViewModel;
        }

        void Handle_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            vm.OnSearchFriendsCommand.Execute(null);
        }
    }
}