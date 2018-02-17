using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Xamarin.Forms;

namespace Journey.Services.Forms
{
    internal class ShareService : Abstractions.Services.Contracts.IShareService
    {
        public async Task ShareText(string text, string title, string url)
        {
            try
            {     
                await CrossShare.Current.Share(new ShareMessage
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

        public async Task ShareImages(string subject, string message, object image)
        {
            try
            {
                IEnumerable<Media> img = image as IEnumerable<Media>;
                IShare shareService= DependencyService.Get<IShare>();
                await shareService.Share(subject,message, img.ToList());
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }

        public async Task ShareVideos(string subject, string message, object video)
        {
            try
            {
                IEnumerable<Media> img = video as IEnumerable<Media>;
                IShare shareService = DependencyService.Get<IShare>();
                await shareService.Share(subject, message, img.ToList());
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }
    }
}