using System;
using System.IO;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Microsoft.WindowsAzure.Storage;

namespace Journey.Services.Buisness.Blob
{
    public class BlobService : IBlobService
    {
        private const string AzureBlobUserName = "journeychallengeblb";

        private const string AzureBlobKey =
            "clmM0pAf+N/xs973g1Bm6wzScrKuZHbPy845uSMCdm27ldE8Vx4hFPTrcupLntKN1rcuSmvvDEK86F4xFYVe5w==";

        private const string AzurePostContainerName = "post";
        private const string AzureBlobBaseUrl = "https://journeychallengeblb.blob.core.windows.net";


        public async Task<string> UploadAsync(Stream stream, string fileName)
        {
            var connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                AzureBlobUserName, AzureBlobKey);

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            var container = blobClient.GetContainerReference("post");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();


            // Retrieve reference to a blob named "myblob".
            var blockBlob = container.GetBlockBlobReference(fileName);

            // Create the "myblob" blob with the text "Hello, world!"
            await blockBlob.UploadFromStreamAsync(stream);

            var fileNewPath = string.Format("{0}/{1}/{2}", AzureBlobBaseUrl, AzurePostContainerName, fileName);

            return fileNewPath;
        }

        public async Task<string> UploadAsync(byte[] bytes, string fileName)
        {
            try
            {
                var stream = new MemoryStream(bytes);
                return await UploadAsync(stream, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}