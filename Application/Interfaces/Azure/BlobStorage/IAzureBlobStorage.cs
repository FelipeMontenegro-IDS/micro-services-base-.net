using Azure.Storage.Blobs.Models;

namespace Application.Interfaces.Azure.BlobStorage;

public interface IAzureBlobStorage
{
    public Task CreateIfNotExistsAsync(string containerName, PublicAccessType accessType = PublicAccessType.None);

    public Task UploadFileAsync(string containerName, string blobName, Stream fileStream,
        IDictionary<string, string>? metadata = null);

    public Task<Stream> DownloadFileAsync(string containerName, string blobName);

    public Task DownloadFileToLocalAsync(string containerName, string blobName, string localFilePath);

    public Task DeleteFileAsync(string containerName, string blobName);

    public Task<List<string>> ListBlobsAsync(string containerName, string? prefix = null);

    public string GetBlobUrl(string containerName, string blobName);

    public Task CopyBlobAsync(string sourceContainerName, string sourceBlobName,
        string destinationContainerName, string destinationBlobName);

    public Task MoveBlobAsync(string sourceContainerName, string sourceBlobName,
        string destinationContainerName, string destinationBlobName);
}