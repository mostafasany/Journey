using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Xamarin.Forms;

namespace Abstractions.Forms
{
    public class ShareService : IShareService
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
                var img = image as IEnumerable<Media>;
                var shareService = DependencyService.Get<IShare>();
                await shareService.Share(subject, message, img.ToList());
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
                var img = video as IEnumerable<Media>;
                var shareService = DependencyService.Get<IShare>();
                await shareService.Share(subject, message, img.ToList());
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }
    }
}