//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Tawasol.ViewModels
//{
//    public class PostCampaignViewModel : PostBaseViewModel
//    {

//        public override async Task OnNavigateTo()
//        {

//        }


//        #region OpenCampainDetailsCommand


//        public RelayCommand<CampaignAccount> OpenCampainDetailsCommand => new RelayCommand<CampaignAccount>(OpenCampainDetails);

//        private void OpenCampainDetails(CampaignAccount account)
//        {
//            if (account == null)
//                return;
//            NavigationService.PushModalAsync(new CampaignGraphPage());
//        }


//        #endregion

//        #region OnPostCampaignLikeCommand


//        public RelayCommand<CampaignAccount> OnPostCampaignLikeCommand => new RelayCommand<CampaignAccount>(OnPostCampaignLike);

//        private async void OnPostCampaignLike(CampaignAccount account)
//        {
//            try
//            {
//                if (account == null)
//                    return;
//                //PostBase campaign = Posts.FirstOrDefault();
//                //if (campaign != null && !(campaign as PostCampaign).IsExpired)
//                account.Liked = !account.Liked;
//            }
//            catch (Exception ex)
//            {
//                ExceptionService.Handle(ex);
//            }
//        }


//        #endregion


//        #region OnCampaignGalleryDetailsCommand


//        public RelayCommand<List<Media>> OnCampaignGalleryDetailsCommand => new RelayCommand<List<Media>>(OnCampaignGalleryDetails);

//        private async void OnCampaignGalleryDetails(List<Media> mediaList)
//        {
//            try
//            {
//                if (mediaList.Any())
//                    NavigationService.PushModalAsync(new MediaPage(mediaList));
//            }
//            catch (Exception ex)
//            {
//                ExceptionService.Handle(ex);
//            }
//        }


//        #endregion
//    }
//}

