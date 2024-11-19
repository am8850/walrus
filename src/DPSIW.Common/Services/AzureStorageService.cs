using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DPSIW.Common.Services
{
    internal class AzureStorageService
    {

        private readonly BlobServiceClient blobServiceClient;

        //public readonly BlobContainerClient containerClient;

        public AzureStorageService(string connecionStr)
        {
            //blobServiceClient = new BlobServiceClient(
            //    new Uri("https://<storage-account-name>.blob.core.windows.net"),
            //    new DefaultAzureCredential());

            blobServiceClient = new BlobServiceClient(connecionStr);
            //containerClient = blobServiceClient.CreateBlobContainer(containerName);
        }

        public async Task UploadBlob(string containerName, string fileName)
        {
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            blobClient.Upload("path to file", true);
        }

        public async Task DownloadBlob(string containerName, string blobName, string targetFilepath)
        {
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DownloadToAsync(targetFilepath);
        }
    }
}
