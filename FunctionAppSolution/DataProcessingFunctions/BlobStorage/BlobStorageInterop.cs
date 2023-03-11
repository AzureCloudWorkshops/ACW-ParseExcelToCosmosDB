using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingFunctions.BlobStorage
{
    public class BlobStorageInterop
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageInterop(string connectionString)
        {
            //create account client
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public byte[] GetBlob(string containerName, string blobName)
        {
            //get the specific container into a client
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if (containerClient == null)
            {
                throw new Exception($"Container {containerName} not found!");
            }

            var fileToProcess = containerClient.GetBlobClient(blobName);

            if (fileToProcess == null || !fileToProcess.Exists())
            {
                throw new Exception($"Blob {blobName} not found in container {containerName}");
            }

            using (var ms = new MemoryStream())
            {
                fileToProcess.DownloadTo(ms);
                return ms.ToArray();
            }
        }
    }
}
