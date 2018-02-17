using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IShareService
    {
        Task ShareText(string text, string title, string url);
        Task ShareImages(string subject, string message, object image);
        Task ShareVideos(string subject, string message, object video);
    }
}