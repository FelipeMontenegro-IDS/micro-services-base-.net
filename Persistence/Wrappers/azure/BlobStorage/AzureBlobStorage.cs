using Application.Interfaces.Azure.BlobStorage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Shared.Utils.Helpers;

namespace Persistence.Wrappers.azure.BlobStorage;

public class AzureBlobStorage : IAzureBlobStorage
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobStorage(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task CreateIfNotExistsAsync(string containerName, PublicAccessType accessType = PublicAccessType.None)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(accessType);
    }

    public async Task UploadFileAsync(string containerName, string blobName, Stream fileStream,
        IDictionary<string, string>? metadata = null)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(fileStream, true);

        if (ValidationHelper.IsNotNull(metadata))
        {
            await blobClient.SetMetadataAsync(metadata);
        }
    }

    public async Task<Stream> DownloadFileAsync(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        var downloadInfo = await blobClient.DownloadAsync();
        return downloadInfo.Value.Content;
    }

    public async Task DownloadFileToLocalAsync(string containerName, string blobName, string localFilePath)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        var downloadInfo = await blobClient.DownloadAsync();

        using (var fileStream = File.OpenWrite(localFilePath))
        {
            await downloadInfo.Value.Content.CopyToAsync(fileStream);
        }
    }

    public async Task DeleteFileAsync(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<List<string>> ListBlobsAsync(string containerName, string? prefix = null)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobs = new List<string>();

        await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: prefix))
        {
            blobs.Add(blobItem.Name);
        }

        return blobs;
    }

    public string GetBlobUrl(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        return blobClient.Uri.ToString();
    }

    public async Task CopyBlobAsync(string sourceContainerName, string sourceBlobName, string destinationContainerName,
        string destinationBlobName)
    {
        var sourceContainerClient = _blobServiceClient.GetBlobContainerClient(sourceContainerName);
        var sourceBlobClient = sourceContainerClient.GetBlobClient(sourceBlobName);
        var destinationContainerClient = _blobServiceClient.GetBlobContainerClient(destinationContainerName);

        await destinationContainerClient.CreateIfNotExistsAsync();

        var destinationBlobClient = destinationContainerClient.GetBlobClient(destinationBlobName);
        await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);
    }

    public async Task MoveBlobAsync(string sourceContainerName, string sourceBlobName, string destinationContainerName,
        string destinationBlobName)
    {
        await CopyBlobAsync(sourceContainerName, sourceBlobName, destinationContainerName, destinationBlobName);
        await DeleteFileAsync(sourceContainerName, sourceBlobName);
    }
}