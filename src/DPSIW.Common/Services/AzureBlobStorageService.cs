using Azure.Storage.Blobs;

namespace DPSIW.Common.Services
{
    public class AzureBlobStorageService
    {

        private readonly BlobServiceClient blobServiceClient;

        //public readonly BlobContainerClient containerClient;

        public AzureBlobStorageService(string connecionStr)
        {
            //blobServiceClient = new BlobServiceClient(
            //    new Uri("https://<storage-account-name>.blob.core.windows.net"),
            //    new DefaultAzureCredential());

            blobServiceClient = new BlobServiceClient(connecionStr);
        }

        public async Task UploadBlob(string containerName, string fileName)
        {
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            blobClient.Upload("path to file", true);
        }

        public async Task DownloadBlob(string blobUri, string localFilePath)
        {
            // Convert blobUri to container name and blob name
            try
            {
                var uri = new Uri(blobUri);

                // Get the container name and blob name
                var (containerName, blobName) = Utilities.Utilities.GetContainerAndName(blobUri);

                // Create a blob client for the blob
                BlobClient blobClient = blobServiceClient
                    .GetBlobContainerClient(containerName)
                    .GetBlobClient(blobName);

                // Download the blob
                Console.WriteLine($"Downloading blob {blobName} to {localFilePath}");
                await blobClient.DownloadToAsync(localFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to download blob: {ex.Message}");
            }

        }
    }
}
