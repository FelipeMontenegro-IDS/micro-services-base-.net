using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Shared.Utils.Generals
{
    public class AzureBlobStorageHelper
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobStorageHelper(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        // Crea un contenedor si no existe
        public async Task CreateContainerIfNotExistsAsync(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        }

        // Carga un archivo al contenedor
        public async Task UploadFileAsync(string containerName, string blobName, Stream fileStream)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(fileStream, true);
        }

        // Descarga un archivo del contenedor
        public async Task<Stream> DownloadFileAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            var downloadInfo = await blobClient.DownloadAsync();
            return downloadInfo.Value.Content;
        }

        // Elimina un archivo del contenedor
        public async Task DeleteFileAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        // Lista todos los blobs en un contenedor
        public async Task<List<string>> ListBlobsAsync(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }

            return blobs;
        }

        // Obtiene la URL de un blob
        public string GetBlobUrl(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            return blobClient.Uri.ToString();
        }

        // Copia un blob a otro contenedor
        public async Task CopyBlobAsync(string sourceContainerName, string sourceBlobName,
            string destinationContainerName, string destinationBlobName)
        {
            var sourceContainerClient = _blobServiceClient.GetBlobContainerClient(sourceContainerName);
            var sourceBlobClient = sourceContainerClient.GetBlobClient(sourceBlobName);
            var destinationContainerClient = _blobServiceClient.GetBlobContainerClient(destinationContainerName);
            await destinationContainerClient.CreateIfNotExistsAsync();

            var destinationBlobClient = destinationContainerClient.GetBlobClient(destinationBlobName);
            await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);
        }

        // Mueve un blob a otro contenedor (copia y elimina)
        public async Task MoveBlobAsync(string sourceContainerName, string sourceBlobName,
            string destinationContainerName, string destinationBlobName)
        {
            await CopyBlobAsync(sourceContainerName, sourceBlobName, destinationContainerName, destinationBlobName);
            await DeleteFileAsync(sourceContainerName, sourceBlobName);
        }
    }
}