using Azure.Storage.Blobs.Models;

namespace Application.Interfaces.Azure.BlobStorage;

public interface IAzureBlobStorage
{
    public Task CreateIfNotExistsAsync(
        string containerName,
        PublicAccessType accessType = PublicAccessType.None,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);


    public Task UploadFileAsync(
        string containerName,
        string blobName,
        Stream fileStream,
        string[] subFolders,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);


    public Task<(Stream Content, string ContentType)> DownloadFileAsync(
        string containerName,
        string blobName,
        string[] subFolders,
        CancellationToken cancellationToken = default);


    public Task DownloadFileToLocalAsync(
        string containerName,
        string blobName,
        string localFilePath,
        string[] subFolders,
        CancellationToken cancellationToken = default);


    public Task DeleteFileAsync(
        string containerName,
        string blobName,
        string[] subFolders,
        CancellationToken cancellationToken = default);


    public Task<List<string>> ListBlobsAsync(string containerName, string? prefix = null,
        CancellationToken cancellationToken = default);


    public string GetBlobUrl(
        string containerName,
        string blobName,
        string[] subFolders);


    public Task CopyBlobAsync(
        string sourceContainerName,
        string sourceBlobName,
        string destinationContainerName,
        string destinationBlobName,
        string[] sourceSubFolders,
        string[] destinationSubFolders,
        CancellationToken cancellationToken = default);


    public Task MoveBlobAsync(
        string sourceContainerName,
        string sourceBlobName,
        string destinationContainerName,
        string destinationBlobName,
        string[] sourceSubFolders,
        string[] destinationSubFolders,
        CancellationToken cancellationToken = default);
}