using System.Collections.Generic;

namespace Abstractions.Services.Contracts
{
    public interface IShareService
    {
        void ShareText(string text, string title, string url);
        void ShareImages(string subject, string message, object image);
        void ShareVideos(string subject, string message, object video);
    }
}