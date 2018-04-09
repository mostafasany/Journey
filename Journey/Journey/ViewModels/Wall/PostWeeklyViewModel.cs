//using System;
//using System.Threading.Tasks;
//using GalaSoft.MvvmLight.Command;
//using Tawasol.Views;

//namespace Tawasol.ViewModels
//{
//    public class PostWeeklyViewModel : PostBaseViewModel
//    {

//        public override async Task OnNavigateTo()
//        {
//        }


//        #region OnAccountDetailsCommand


//        public RelayCommand<Account> OnAccountDetailsCommand => new RelayCommand<Account>(OnProfileDetails);

//        private void OnProfileDetails(Account account)
//        {
//            try
//            {
//                if (account == null)
//                    return;

//                if (account != null)
//                    NavigationService.NavigateToPage(typeof(FriendsPage), account.Id);
//            }
//            catch (Exception ex)
//            {
//                ExceptionService.Handle(ex);
//            }
//        }


//        #endregion


//    }
//}

