//using System;
//using System.Collections.ObjectModel;
//using Tawasol.Helpers;
//using Xamarin.Forms;

//namespace Tawasol.Models
//{
//    public class PostCampaign : PostBase
//    {
//        public PostCampaign()
//        {
//            Device.StartTimer(TimeSpan.FromSeconds(1), HandleFunc);
//        }

//        bool HandleFunc()
//        {
//            if (IsNotExpired)
//                Timer = DateHelper.Format(--ToExpire);
//            return true;
//        }

//        private ObservableCollection<CampaignAccount> campaignAccount;
//        public ObservableCollection<CampaignAccount> CampaignAccounts
//        {
//            get => campaignAccount;
//            set => SetProperty(ref campaignAccount, value);
//        }

//        private string message;
//        public string Message
//        {
//            get => message;
//            set => SetProperty(ref message, value);
//        }

//        //In Min
//        public double ToExpire { get; set; }

//        private string timer;
//        public string Timer
//        {
//            get => timer;
//            set
//            {
//                SetProperty(ref timer, value);
//                RaisePropertyChanged("IsExpired");
//                RaisePropertyChanged("Opacity");
//            }
//        }

//        public bool IsExpired => ToExpire <= 0;

//        public bool IsNotExpired => ToExpire > 0;

//        public double Opacity => IsExpired ? 0.2 : 1;
//    }
//}
