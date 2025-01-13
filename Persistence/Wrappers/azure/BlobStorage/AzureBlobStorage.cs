using Application.Interfaces.Azure.BlobStorage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Shared.Interfaces.Helpers;

namespace Persistence.Wrappers.azure.BlobStorage;

public class AzureBlobStorage : IAzureBlobStorage
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IValidationHelper _validationHelper;

    public AzureBlobStorage(BlobServiceClient blobServiceClient, IValidationHelper validationHelper)
    {
        _blobServiceClient = blobServiceClient;
        _validationHelper = validationHelper;
    }

    /// <summary>
    /// Crea un contenedor en Azure Blob Storage si no existe.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor a crear.</param>
    /// <param name="accessType">El tipo de acceso público para el contenedor. Por defecto es <c>PublicAccessType.None</c>.</param>
    /// <param name="metadata">Metadatos opcionales para el contenedor.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>
    /// Una tarea vacía que representa la operación asincrónica.
    /// </returns>
    public async Task CreateIfNotExistsAsync(
        string containerName,
        PublicAccessType accessType = PublicAccessType.None,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(accessType, metadata, cancellationToken);
    }

    /// <summary>
    /// Sube un archivo a un contenedor en Azure Blob Storage.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor donde se subirá el archivo.</param>
    /// <param name="blobName">El nombre del blob que se creará.</param>
    /// <param name="fileStream">El flujo del archivo que se subirá.</param>
    /// <param name="subFolders">Sub-carpetas opcionales dentro del contenedor.</param>
    /// <param name="metadata">Metadatos opcionales para el blob.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>
    /// Una tarea vacía que representa la operación asincrónica.
    /// </returns>
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
        
        // var uploadOptions = new BlobHttpHeaders { ContentType = contentType, };

        await blobClient.UploadAsync(fileStream, true, cancellationToken);

        if (_validationHelper.IsNotNull(metadata))
        {
            await blobClient.SetMetadataAsync(metadata, null, cancellationToken);
        }
    }

    /// <summary>
    /// Descarga un archivo de un contenedor en Azure Blob Storage.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor desde el cual se descargará el archivo.</param>
    /// <param name="blobName">El nombre del blob que se descargará.</param>
    /// <param name="subFolders">Sub-carpetas opcionales dentro del contenedor.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>
    /// Un flujo que representa el archivo descargado y el content-type del mismo.
    /// </returns>
    public async Task<(Stream Content, string ContentType)> DownloadFileAsync(
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
        
        string contentType = downloadInfo.Value.ContentType;
        return (downloadInfo.Value.Content, contentType);
    }

    /// <summary>
    /// Descarga un archivo de un contenedor en Azure Blob Storage a una ruta local.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor desde el cual se descargará el archivo.</param>
    /// <param name="blobName">El nombre del blob que se descargará.</param>
    /// <param name="localFilePath">La ruta local donde se guardará el archivo.</param>
    /// <param name="subFolders">Sub-carpetas opcionales dentro del contenedor.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>
    /// Una tarea vacía que representa la operación asincrónica.
    /// </returns>
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

    /// <summary>
    /// Elimina un archivo de un contenedor en Azure Blob Storage.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor del cual se eliminará el archivo.</param>
    /// <param name="blobName">El nombre del blob que se eliminará.</param>
    /// <param name="subFolders">Sub-carpetas opcionales dentro del contenedor.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>
    /// Una tarea vacía que representa la operación asincrónica.
    /// </returns>
    /// 
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

    /// <summary>
    /// Lista los blobs en un contenedor de Azure Blob Storage.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor del cual se listarán los blobs.</param>
    /// <param name="prefix">Prefijo opcional para filtrar los blobs listados.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>
    /// Una lista de nombres de blobs en el contenedor.
    /// </returns>
    public async Task<List<string>> ListBlobsAsync(string containerName, string? prefix = null,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobs = new List<string>();

        await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: prefix,
                           cancellationToken: cancellationToken))
        {
            blobs.Add(blobItem.Name);
        }

        return blobs;
    }

    /// <summary>
    /// Obtiene la URL de un blob en Azure Blob Storage.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor que contiene el blob.</param>
    /// <param name="blobName">El nombre del blob.</param>
    /// <param name="subFolders">Sub-carpetas opcionales dentro del contenedor.</param>
    /// <returns>
    /// La URL del blob.
    /// </returns>
    public string GetBlobUrl(string containerName, string blobName, string[] subFolders)
    {
        string folderPath = string.Join("/", subFolders);
        string fullBlobName = string.IsNullOrEmpty(folderPath) ? blobName : $"{folderPath}/{blobName}";

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fullBlobName);
        return blobClient.Uri.ToString();
    }

    /// <summary>
    /// Copia un blob de un contenedor a otro en Azure Blob Storage.
    /// </summary>
    /// <param name="sourceContainerName">El nombre del contenedor de origen.</param>
    /// <param name="sourceBlobName">El nombre del blob de origen.</param>
    /// <param name="destinationContainerName">El nombre del contenedor de destino.</param>
    /// <param name="destinationBlobName">El nombre del blob de destino.</param>
    /// <param name="sourceSubFolders">Sub-carpetas opcionales en el contenedor de origen.</param>
    /// <param name="destinationSubFolders">Sub-carpetas opcionales en el contenedor de destino.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>
    /// Una tarea vacía que representa la operación asincrónica.
    /// </returns>
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

    /// <summary>
    /// Mueve un blob de un contenedor a otro en Azure Blob Storage.
    /// </summary>
    /// <param name="sourceContainerName">El nombre del contenedor de origen.</param>
    /// <param name="sourceBlobName">El nombre del blob de origen.</param>
    /// <param name="destinationContainerName">El nombre del contenedor de destino.</param>
    /// <param name="destinationBlobName">El nombre del blob de destino.</param>
    /// <param name="sourceSubFolders">Sub-carpetas opcionales en el contenedor de origen.</param>
    /// <param name="destinationSubFolders">Sub-carpetas opcionales en el contenedor de destino.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>
    /// Una tarea vacía que representa la operación asincrónica.
    /// </returns>
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