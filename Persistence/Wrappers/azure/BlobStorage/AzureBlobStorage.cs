using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Persistence.Interfaces.Azure.BlobStorage;
using Shared.Helpers;

namespace Persistence.Wrappers.azure.BlobStorage;

public class AzureBlobStorage : IAzureBlobStorage
{
      private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobStorage(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task CreateIfNotExistsAsync(
        string containerName,
        PublicAccessType accessType = PublicAccessType.None,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(accessType, metadata, cancellationToken);
    }

    public async Task UploadFileAsync(
        string containerName,
        string blobName,
        Stream fileStream,
        string[] subFolders,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        string folderPath = string.Join("/", subFolders);
        string fullBlobName = string.IsNullOrEmpty(folderPath) ? blobName : $"{folderPath}/{blobName}";

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fullBlobName);
        await blobClient.UploadAsync(fileStream, true, cancellationToken);

        if (ValidationHelper.IsNotNull(metadata))
        {
            await blobClient.SetMetadataAsync(metadata, null, cancellationToken);
        }
    }

    public async Task<Stream> DownloadFileAsync(
        string containerName,
        string blobName,
        string[] subFolders,
        CancellationToken cancellationToken = default)
    {
        string folderPath = string.Join("/", subFolders);
        string fullBlobName = string.IsNullOrEmpty(folderPath) ? blobName : $"{folderPath}/{blobName}";

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fullBlobName);
        var downloadInfo = await blobClient.DownloadAsync(cancellationToken);
        return downloadInfo.Value.Content;
    }

    public async Task DownloadFileToLocalAsync(
        string containerName,
        string blobName,
        string localFilePath,
        string[] subFolders,
        CancellationToken cancellationToken = default)
    {
        string folderPath = string.Join("/", subFolders);
        string fullBlobName = string.IsNullOrEmpty(folderPath) ? blobName : $"{folderPath}/{blobName}";

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fullBlobName);
        var downloadInfo = await blobClient.DownloadAsync(cancellationToken);

        var fileStream = File.OpenWrite(localFilePath);
        try
        {
            await downloadInfo.Value.Content.CopyToAsync(fileStream, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new FileLoadException(ex.Message);
        }
        finally
        {
            await fileStream.DisposeAsync();
        }
    }

    public async Task DeleteFileAsync(
        string containerName, 
        string blobName, 
        string[] subFolders,
        CancellationToken cancellationToken = default)
    {
        // Construir la ruta del blob
        string folderPath = string.Join("/", subFolders);
        string fullBlobName = string.IsNullOrEmpty(folderPath) ? blobName : $"{folderPath}/{blobName}";

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fullBlobName);
        await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.None, null, cancellationToken);
    }

    public async Task<List<string>> ListBlobsAsync(string containerName, string? prefix = null,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobs = new List<string>();

        await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: prefix, cancellationToken: cancellationToken))
        {
            blobs.Add(blobItem.Name);
        }

        return blobs;
    }

    public string GetBlobUrl(string containerName, string blobName, string[] subFolders)
    {
        string folderPath = string.Join("/", subFolders);
        string fullBlobName = string.IsNullOrEmpty(folderPath) ? blobName : $"{folderPath}/{blobName}";

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fullBlobName);
        return blobClient.Uri.ToString();
    }

    public async Task CopyBlobAsync(
        string sourceContainerName,
        string sourceBlobName,
        string destinationContainerName,
        string destinationBlobName,
        string[] sourceSubFolders,
        string[] destinationSubFolders,
        CancellationToken cancellationToken = default)
    {
        // Construir la ruta del blob de origen
        string sourceFolderPath = string.Join("/", sourceSubFolders);
        string fullSourceBlobName = string.IsNullOrEmpty(sourceFolderPath)
            ? sourceBlobName
            : $"{sourceFolderPath}/{sourceBlobName}";

        var sourceContainerClient = _blobServiceClient.GetBlobContainerClient(sourceContainerName);
        var sourceBlobClient = sourceContainerClient.GetBlobClient(fullSourceBlobName);
        var destinationContainerClient = _blobServiceClient.GetBlobContainerClient(destinationContainerName);

        await destinationContainerClient.CreateIfNotExistsAsync(PublicAccessType.None, null, null, cancellationToken);

        // Construir la ruta del blob de destino
        string destinationFolderPath = string.Join("/", destinationSubFolders);
        string fullDestinationBlobName = string.IsNullOrEmpty(destinationFolderPath)
            ? destinationBlobName
            : $"{destinationFolderPath}/{destinationBlobName}";

        var destinationBlobClient = destinationContainerClient.GetBlobClient(fullDestinationBlobName);
        await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri, null, cancellationToken);
    }

    public async Task MoveBlobAsync(
        string sourceContainerName,
        string sourceBlobName,
        string destinationContainerName,
        string destinationBlobName,
        string[] sourceSubFolders,
        string[] destinationSubFolders,
        CancellationToken cancellationToken = default)
    {
        await CopyBlobAsync(sourceContainerName, sourceBlobName, destinationContainerName, destinationBlobName,
            sourceSubFolders, destinationSubFolders, cancellationToken);
        await DeleteFileAsync(sourceContainerName, sourceBlobName, sourceSubFolders, cancellationToken);
    }
}