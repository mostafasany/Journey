using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Plugin.Share;

namespace Journey.Services.Forms
{
    internal class ShareService : IShareService
    {
        public void Share(string text, string title, string url)
        {
            try
            {
                CrossShare.Current.Share(new Plugin.Share.Abstractions.ShareMessage
                {
                    Text = text,
                    Title = title,
                    Url = url
                });
            }
            catch (System.Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }
    }
}