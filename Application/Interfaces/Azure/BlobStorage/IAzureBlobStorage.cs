using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Shared.Enums.Data;
using Shared.Enums.Time;

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
        string[]? subFolders = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);

    public Task UploadFileFromBase64Async(
        string containerName,
        string blobName,
        string base64String,
        string[]? subFolders = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);
    
    public Task UploadFileInBlocksAsync(
        string containerName,
        string blobName,
        Stream fileStream,
        FileSize fileSize = FileSize.Mb50,
        string[]? subFolders = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);
    
    public Task UploadFileInBlocksFromBase64Async(
        string containerName,
        string blobName,
        string base64String,
        FileSize fileSize = FileSize.Mb50,
        string[]? subFolders = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);

    public Task<(Stream Content, string ContentType)> DownloadFileAsync(
        string containerName,
        string blobName,
        string[]? subFolders = null,
        CancellationToken cancellationToken = default);

    public Task<(Stream Content, string ContentType)> DownloadFileBlockAsync(
        string containerName,
        string blobName,
        string[]? subFolders = null,
        CancellationToken cancellationToken = default);


    public Task DownloadFileToLocalAsync(
        string containerName,
        string blobName,
        string localFilePath,
        string[]? subFolders = null,
        CancellationToken cancellationToken = default);


    public Task DeleteFileAsync(
        string containerName,
        string blobName,
        string[]? subFolders = null,
        CancellationToken cancellationToken = default);


    public Task<List<string>> ListBlobsAsync(
        string containerName,
        string? prefix = null,
        CancellationToken cancellationToken = default);


    public string GetBlobUrl(
        string containerName,
        string blobName,
        string[]? subFolders = null);

    public string GetBlobUrlBase64(
        string containerName,
        string blobName,
        string[]? subFolders = null);


    public Task CopyBlobAsync(
        string sourceContainerName,
        string sourceBlobName,
        string destinationContainerName,
        string destinationBlobName,
        string[]? sourceSubFolders = null,
        string[]? destinationSubFolders = null,
        CancellationToken cancellationToken = default);


    public Task MoveBlobAsync(
        string sourceContainerName,
        string sourceBlobName,
        string destinationContainerName,
        string destinationBlobName,
        string[]? sourceSubFolders = null,
        string[]? destinationSubFolders = null,
        CancellationToken cancellationToken = default);

    public string GenerateBlobSas(
        string containerName,
        string blobName,
        TimeSpan expiryDuration,
        BlobSasPermissions permissions,
        string[]? subFolders = null,
        SasProtocol sasProtocol = SasProtocol.Https,
        TimeZoneOption timeZone = TimeZoneOption.Utc);

    public string GenerateContainerSas(
        string containerName,
        TimeSpan expiryDuration,
        BlobContainerSasPermissions permissions,
        SasProtocol sasProtocol = SasProtocol.Https,
        TimeZoneOption timeZone = TimeZoneOption.Utc);
}