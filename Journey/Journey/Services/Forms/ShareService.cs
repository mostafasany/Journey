using System;
using Abstractions.Exceptions;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Xamarin.Forms;

namespace Journey.Services.Forms
{
    internal class ShareService : Abstractions.Services.Contracts.IShareService
    {
        public void Share(string text, string title, string url)
        {
            try
            {
                           
                CrossShare.Current.Share(new ShareMessage
                {
                    Text = text,
                    Title = title,
                    Url = url
                });
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }

        public void Share(string subject, string message, object image)
        {
            try
            {
                ImageSource img = image as ImageSource;
                IShare shareService= DependencyService.Get<IShare>();
                shareService.Share(subject,message, img);
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
          
                
        }
    }
}