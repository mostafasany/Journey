using Abstractions.Forms;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Xamarin.Forms;
using ShareService = Journey.UWP.Services.ShareService;

[assembly: Dependency(typeof(ShareService))]

namespace Journey.UWP.Services
{
    public class ShareService : IShare
    {
        private StorageFile _sharedStorageFolder;
        public ShareService()
        {
            RegisterForShare();
        }

        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += ShareImageHandler;
        }

        private void ShareImageHandler(DataTransferManager sender,
            DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "Share Image Example";
            request.Data.Properties.Description = "Demonstrates how to share an image.";

            // Because we are making async calls in the DataRequested event handler,
            //  we need to get the deferral first.
            DataRequestDeferral deferral = request.GetDeferral();

            // Make sure we always call Complete on the deferral.
            try
            {
                //StorageFile thumbnailFile =
                //    await Package.Current.InstalledLocation.GetFileAsync("Assets\\SmallLogo.png");
                //request.Data.Properties.Thumbnail =
                //    RandomAccessStreamReference.CreateFromFile(thumbnailFile);
                //StorageFile imageFile =
                //    await Package.Current.InstalledLocation.GetFileAsync("Assets\\Logo.png");
                request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(_sharedStorageFolder));
            }
            finally
            {
                deferral.Complete();
            }
        }


        public async Task Share(string subject, string message, List<Media> mediaItems)
        {
            foreach (Media media in mediaItems)
            {
                _sharedStorageFolder = await DownloadFile(media);
                DataTransferManager.ShowShareUI();
            }
        }

        private async Task<StorageFile> DownloadFile(Media media)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(media.Path);

            byte[] img = await response.Content.ReadAsByteArrayAsync();

            return await AsStorageFile(img, Guid.NewGuid() + media.Ext);
        }

        private static async Task<StorageFile> AsStorageFile(byte[] byteArray, string fileName)
        {
            StorageFolder storageFolder = ApplicationData.Current.TemporaryFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(sampleFile, byteArray);
            return sampleFile;
        }
    }
}