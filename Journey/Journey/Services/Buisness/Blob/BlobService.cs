using System;
using System.IO;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;

namespace Journey.Services.Buisness.Blob
{
    public class BlobService : IBlobService
    {
        public Task<string> UploadAsync(Stream stream, string name)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadAsync(byte[] bytes, string name)
        {
            throw new NotImplementedException();
        }
    }
}