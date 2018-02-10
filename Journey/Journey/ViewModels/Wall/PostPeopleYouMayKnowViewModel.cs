//using System;
//using System.Threading.Tasks;
//using GalaSoft.MvvmLight.Command;
//using Microsoft.Practices.ServiceLocation;
//using Tawasol.Services;
//using Tawasol.Views;

//namespace Tawasol.ViewModels
//{
//    public class PostPeopleYouMayKnowViewModel : PostBaseViewModel
//    {
//        readonly IFriendService friendService;
//        public PostPeopleYouMayKnowViewModel()
//        {
//            friendService = ServiceLocator.Current.GetInstance<IFriendService>();
//        }

//        public override async Task OnNavigateTo()
//        {
//        }

//        #region OnPeopleYouMayKnowCommand


//        public RelayCommand<Account> OnPeopleYouMayKnowCommand => new RelayCommand<Account>(OnPeopleYouMayKnow);

//        private void OnPeopleYouMayKnow(Account account)
//        {
//            if (account == null)
//                return;

//            NavigationService.NavigateToPage(typeof(FriendsPage), account.Id);
//        }


//        #endregion

//        #region OnFollowCommand


//        public RelayCommand<Account> OnFollowCommand => new RelayCommand<Account>(OnFollow);

//        private async void OnFollow(Account account)
//        {
//            try
//            {
//                if (account == null)
//                    return;
//                account.Following = !account.Following;
//                LoggedInAccount = await AccountService.GetAccountAsync();
//                var failureIds = await friendService.FollowAsync(new System.Collections.Generic.List<string>() { account.Id });
//                if (failureIds?.Count != 0)
//                {
//                    DialogService.DisplayAlert("Error", "Cant follow now");
//                }
//            }
//            catch (Exception ex)
//            {
//                ExceptionService.Handle(ex);
//            }
//        }


//        #endregion

//    }
//}

