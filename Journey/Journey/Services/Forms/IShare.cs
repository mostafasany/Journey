using System.Collections.Generic;
using Xamarin.Forms;

namespace Journey.Services.Forms
{
    public interface IShare
    {
        void Share(string subject, string message, List<ImageSource> image);
    }
}
