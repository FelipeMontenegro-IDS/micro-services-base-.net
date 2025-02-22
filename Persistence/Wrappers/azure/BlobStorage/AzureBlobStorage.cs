using System.Text;
using Application.Interfaces.Azure.BlobStorage;
using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Shared.Configurations;
using Shared.Constants.Data;
using Shared.Enums.Data;
using Shared.Enums.Time;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Data;

namespace Persistence.Wrappers.azure.BlobStorage;

public class AzureBlobStorage : IAzureBlobStorage
{
    private readonly AzureBlobStorageOption _config;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IValidationHelper _validationHelper;
    private readonly IDateTimeHelper _dateTimeHelper;
    private readonly IFileSizeProvider _fileSizeProvider;
    private readonly IPathHelper _pathHelper;

    public AzureBlobStorage(
        IOptions<AzureBlobStorageOption> config,
        BlobServiceClient blobServiceClient,
        IValidationHelper validationHelper,
        IDateTimeHelper dateTimeHelper,
        IFileSizeProvider fileSizeProvider,
        IPathHelper pathHelper)
    {
        _config = config.Value ?? throw new ArgumentNullException(nameof(config));
        _blobServiceClient = blobServiceClient ?? throw new ArgumentNullException(nameof(blobServiceClient));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
        _dateTimeHelper = dateTimeHelper ?? throw new ArgumentNullException(nameof(dateTimeHelper));
        _fileSizeProvider = fileSizeProvider ?? throw new ArgumentNullException(nameof(fileSizeProvider));
        _pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
    }


    public async Task CreateIfNotExistsAsync(
        string containerName,
        PublicAccessType accessType = PublicAccessType.None,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(accessType, metadata, cancellationToken);
    }


    public async Task UploadFileAsync(
        string containerName,
        string blobName,
        Stream fileStream,
        string[]? subFolders = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        string folderPath = _pathHelper.BuildFolderPath(subFolders);
        string fullBlobName = _pathHelper.BuildFullPath(blobName, folderPath);

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fullBlobName);

        // var uploadOptions = new BlobHttpHeaders { ContentType = contentType, };

        await blobClient.UploadAsync(fileStream, true, cancellationToken);

        if (_validationHelper.IsNotNull(metadata))
        {
            await blobClient.SetMetadataAsync(metadata, cancellationToken: cancellationToken);
        }
    }

    public async Task UploadFileFromBase64Async(
        string containerName,
        string blobName,
        string base64String,
        string[]? subFolders = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        byte[] fileBytes = Convert.FromBase64String(base64String);
        using MemoryStream memoryStream = new MemoryStream(fileBytes);

        await UploadFileAsync(containerName, blobName, memoryStream, subFolders, metadata, cancellationToken);
    }

    public async Task UploadFileInBlocksAsync(
        string containerName,
        string blobName,
        Stream fileStream,
        FileSize fileSize = FileSize.Mb50,
        string[]? subFolders = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        string folderPath = _pathHelper.BuildFolderPath(subFolders);
        string fullBlobName = _pathHelper.BuildFullPath(blobName, folderPath);

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlockBlobClient blobClient = containerClient.GetBlockBlobClient(fullBlobName);

        List<string> blockIds = new List<string>();
        int blobSize = (int)_fileSizeProvider.GetValue(fileSize, FileSizeConstant.Mb50);
        byte[] buffer = new byte[blobSize];

        int bytesRead;
        int blockNumber = 0;

        try
        {
            while ((bytesRead = await fileStream.ReadAsync(buffer, 0, blobSize, cancellationToken)) > 0)
            {
                string blockId = Convert.ToBase64String(BitConverter.GetBytes(blockNumber));

                using MemoryStream memoryStream = new MemoryStream(buffer, 0, bytesRead);
                await blobClient.StageBlockAsync(blockId, memoryStream, cancellationToken: cancellationToken);

                blockIds.Add(blockId);
                blockNumber++;
            }

            await blobClient.CommitBlockListAsync(blockIds, metadata: metadata, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("Error loading the file in blocks.", e);
        }
    }

    public async Task UploadFileInBlocksFromBase64Async(
        string containerName,
        string blobName,
        string base64String,
        FileSize fileSize = FileSize.Mb50,
        string[]? subFolders = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        byte[] fileBytes = Convert.FromBase64String(base64String);
        using MemoryStream memoryStream = new MemoryStream(fileBytes);

        await UploadFileInBlocksAsync(
            containerName,
            blobName,
            memoryStream,
            fileSize,
            subFolders,
            metadata,
            cancellationToken);
    }


    public async Task<(Stream Content, string ContentType)> DownloadFileAsync(
        string containerName,
        string blobName,
        string[]? subFolders = null,
        CancellationToken cancellationToken = default)
    {
        string folderPath = _pathHelper.BuildFolderPath(subFolders);
        string fullBlobName = _pathHelper.BuildFullPath(blobName, folderPath);

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fullBlobName);

        if (!await blobClient.ExistsAsync(cancellationToken))
        {
            throw new FileNotFoundException($"El blob '{fullBlobName}' no existe en el contenedor '{containerName}'.");
        }

        Response<BlobDownloadInfo> downloadInfo = await blobClient.DownloadAsync(cancellationToken);

        string contentType = downloadInfo.Value.ContentType;
        return (downloadInfo.Value.Content, contentType);
    }

    public async Task<(Stream Content, string ContentType)> DownloadFileBlockAsync(
        string containerName,
        string blobName,
        string[]? subFolders = null,
        CancellationToken cancellationToken = default)
    {
        string folderPath = _pathHelper.BuildFolderPath(subFolders);
        string fullBlobName = _pathHelper.BuildFullPath(blobName, folderPath);

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlockBlobClient blobClient = containerClient.GetBlockBlobClient(fullBlobName);

        try
        {
            Response<BlobDownloadInfo> response = await blobClient.DownloadAsync(cancellationToken: cancellationToken);
            return (response.Value.Content, response.Value.ContentType);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Error al descargar el archivo: {e.Message}", e);
        }
    }


    public async Task DownloadFileToLocalAsync(
        string containerName,
        string blobName,
        string localFilePath,
        string[]? subFolders = null,
        CancellationToken cancellationToken = default)
    {
        string folderPath = _pathHelper.BuildFolderPath(subFolders);
        string fullBlobName = _pathHelper.BuildFullPath(blobName, folderPath);

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fullBlobName);
        Response<BlobDownloadInfo> downloadInfo = await blobClient.DownloadAsync(cancellationToken);

        FileStream fileStream = File.OpenWrite(localFilePath);
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
        string[]? subFolders = null,
        CancellationToken cancellationToken = default)
    {
        string folderPath = _pathHelper.BuildFolderPath(subFolders);
        string fullBlobName = _pathHelper.BuildFullPath(blobName, folderPath);

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fullBlobName);

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }


    public async Task<List<string>> ListBlobsAsync(
        string containerName,
        string? prefix = null,
        CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        List<string> blobs = new List<string>();

        await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: prefix,
                           cancellationToken: cancellationToken))
        {
            blobs.Add(blobItem.Name);
        }

        return blobs;
    }


    public string GetBlobUrl(string containerName, string blobName, string[]? subFolders = null)
    {
        string folderPath = _pathHelper.BuildFolderPath(subFolders);
        string fullBlobName = _pathHelper.BuildFullPath(blobName, folderPath);

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fullBlobName);

        return blobClient.Uri.ToString();
    }

    public string GetBlobUrlBase64(string containerName, string blobName, string[]? subFolders = null)
    {
        string url = GetBlobUrl(containerName, blobName, subFolders);
        string base64Url = Convert.ToBase64String(Encoding.UTF8.GetBytes(url));

        return base64Url;
    }


    public async Task CopyBlobAsync(
        string sourceContainerName,
        string sourceBlobName,
        string destinationContainerName,
        string destinationBlobName,
        string[]? sourceSubFolders = null,
        string[]? destinationSubFolders = null,
        CancellationToken cancellationToken = default)
    {
        string sourceFolderPath = _pathHelper.BuildFolderPath(sourceSubFolders);
        string fullSourceBlobName = _pathHelper.BuildFullPath(sourceBlobName, sourceFolderPath);

        BlobContainerClient sourceContainerClient = _blobServiceClient.GetBlobContainerClient(sourceContainerName);
        BlobClient sourceBlobClient = sourceContainerClient.GetBlobClient(fullSourceBlobName);
        BlobContainerClient destinationContainerClient =
            _blobServiceClient.GetBlobContainerClient(destinationContainerName);

        await destinationContainerClient.CreateIfNotExistsAsync(publicAccessType: PublicAccessType.None,
            cancellationToken: cancellationToken);

        string destinationFolderPath = _pathHelper.BuildFolderPath(destinationSubFolders);
        string fullDestinationBlobName = _pathHelper.BuildFullPath(destinationBlobName, destinationFolderPath);

        BlobClient destinationBlobClient = destinationContainerClient.GetBlobClient(fullDestinationBlobName);

        await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri, cancellationToken: cancellationToken);
    }


    public async Task MoveBlobAsync(
        string sourceContainerName,
        string sourceBlobName,
        string destinationContainerName,
        string destinationBlobName,
        string[]? sourceSubFolders = null,
        string[]? destinationSubFolders = null,
        CancellationToken cancellationToken = default)
    {
        await CopyBlobAsync(
            sourceContainerName,
            sourceBlobName,
            destinationContainerName,
            destinationBlobName,
            sourceSubFolders,
            destinationSubFolders,
            cancellationToken);

        await DeleteFileAsync(sourceContainerName, sourceBlobName, sourceSubFolders, cancellationToken);
    }

    public string GenerateBlobSas(
        string containerName,
        string blobName,
        TimeSpan expiryDuration,
        BlobSasPermissions permissions,
        string[]? subFolders = null,
        SasProtocol sasProtocol = SasProtocol.Https,
        TimeZoneOption timeZone = TimeZoneOption.Utc)
    {
        string folderPath = _pathHelper.BuildFolderPath(subFolders);
        string fullBlobName = _pathHelper.BuildFullPath(blobName, folderPath);

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fullBlobName);

        if (!blobClient.Exists())
        {
            throw new FileNotFoundException(
                $"The blob '{fullBlobName}' does not exist in the container '{containerName}'.");
        }

        DateTimeOffset expirationTime = DateTimeOffset.UtcNow; // Hora actual en UTC
        DateTimeOffset localExpirationTime = _dateTimeHelper.ConvertTo(expirationTime, timeZone).Add(expiryDuration);

        BlobSasBuilder sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = blobName,
            ExpiresOn = localExpirationTime,
            Protocol = sasProtocol
        };
        sasBuilder.SetPermissions(permissions);

        string accountName = _config.AccountName;
        string accountKey = _config.AccountKey;

        var sasToken = sasBuilder.ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(accountName, accountKey)).ToString();
        return $"{blobClient.Uri}?{sasToken}";
    }

    public string GenerateContainerSas(
        string containerName,
        TimeSpan expiryDuration,
        BlobContainerSasPermissions permissions,
        SasProtocol sasProtocol = SasProtocol.Https,
        TimeZoneOption timeZone = TimeZoneOption.Utc)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        if (!containerClient.Exists())
        {
            throw new DirectoryNotFoundException($"El contenedor '{containerName}' no existe.");
        }

        DateTimeOffset expirationTime = DateTimeOffset.UtcNow;
        DateTimeOffset localExpirationTime = _dateTimeHelper.ConvertTo(expirationTime, timeZone).Add(expiryDuration);

        BlobSasBuilder sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            ExpiresOn = localExpirationTime,
            Protocol = sasProtocol
        };
        sasBuilder.SetPermissions(permissions);

        string accountName = _config.AccountName;
        string accountKey = _config.AccountKey;


        var sasToken = sasBuilder.ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(accountName, accountKey)).ToString();
        return $"{containerClient.Uri}?{sasToken}";
    }
}