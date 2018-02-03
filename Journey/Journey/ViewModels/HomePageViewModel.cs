//using System;
//using System.Windows.Input;
//using Prism.Commands;
//using Unity;

//namespace ViewModels
//{
//    public class HomePageViewModel : BaseViewModel
//    {
//        public HomePageViewModel(IUnityContainer container) :
//            base(container)
//        {
//        }

//        #region Events

//        public override void OnNavigatedTo(object paramater, bool isBack)
//        {
//            base.OnNavigatedTo(paramater, isBack);
//            Intialize();
//        }

//        public override void OnNavigatingFrom()
//        {
//            base.OnNavigatingFrom();
//        }

//        public override void OnNavigatedFrom()
//        {
//            base.OnNavigatedFrom();
//        }

//        public override void OnBackPressed()
//        {
//            base.OnBackPressed();
//        }

//        #endregion

//        #region Properties

//        private string _searchText;

//        public string SearchText
//        {
//            get => _searchText;
//            set => SetProperty(ref _searchText, value);
//        }

//        #endregion

//        #region Methods

//        public override void Intialize()
//        {
//            try
//            {
//                ShowProgress();
//                base.Intialize();
//            }
//            catch (Exception e)
//            {
//                ExceptionService.HandleAndShowDialog(e);
//            }
//            finally
//            {
//                HideProgress();
//            }
//        }

//        protected override void Cleanup()
//        {
//            try
//            {
//                //Here Cleanup any resources
//                base.Cleanup();
//            }
//            catch (Exception e)
//            {
//                ExceptionService.HandleAndShowDialog(e);
//            }
//        }

//        #endregion

//        #region Commands

//        #region OpenFilterCommand

//        private ICommand _openFilterCommand;

//        public ICommand OpenFilterCommand => _openFilterCommand ??
//                                             (_openFilterCommand =
//                                                 new DelegateCommand(OpenFilter));

//        private void OpenFilter()
//        {
//            try
//            {
//            }
//            catch (Exception e)
//            {
//                ExceptionService.HandleAndShowDialog(e);
//            }
//            finally
//            {
//                HideProgress();
//            }
//        }

//        #endregion

//        #endregion
//    }
//}