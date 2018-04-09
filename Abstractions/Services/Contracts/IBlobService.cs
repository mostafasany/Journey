using System.IO;
using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IBlobService
    {
        Task<string> UploadAsync(Stream stream, string fileName);
        Task<string> UploadAsync(byte[] bytes, string fileName);
    }
}