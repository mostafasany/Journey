using System;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace Journey.Services.Forms
{
    internal class ShareService : IShareService
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
    }
}