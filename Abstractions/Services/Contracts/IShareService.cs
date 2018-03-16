using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IShareService
    {
        Task ShareImages(string subject, string message, object image);
        Task ShareText(string text, string title, string url);
        Task ShareVideos(string subject, string message, object video);
    }
}