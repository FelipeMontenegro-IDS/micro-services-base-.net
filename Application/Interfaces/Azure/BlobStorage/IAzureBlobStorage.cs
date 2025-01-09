using Azure.Storage.Blobs.Models;

namespace Application.Interfaces.Azure.BlobStorage;

/// <summary>
/// Interfaz que define las operaciones para interactuar con Azure Blob Storage.
/// </summary>
public interface IAzureBlobStorage
{
    
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
    
    public Task CreateIfNotExistsAsync(
        string containerName, 
        PublicAccessType accessType = PublicAccessType.None,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);

    
    
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
    
    public Task UploadFileAsync(
        string containerName,
        string blobName,
        Stream fileStream,
        string[] subFolders,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);

    
    /// <summary>
    /// Descarga un archivo de un contenedor en Azure Blob Storage.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor desde el cual se descargará el archivo.</param>
    /// <param name="blobName">El nombre del blob que se descargará.</param>
    /// <param name="subFolders">Sub-carpetas opcionales dentro del contenedor.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>
    /// Un flujo que representa el archivo descargado.
    /// </returns>
    
    public Task<Stream> DownloadFileAsync(
        string containerName, 
        string blobName, 
        string[] subFolders,
        CancellationToken cancellationToken = default);

    
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
    
    public Task DownloadFileToLocalAsync(
        string containerName,
        string blobName,
        string localFilePath,
        string[] subFolders,
        CancellationToken cancellationToken = default);

    
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
    public Task DeleteFileAsync(
        string containerName, 
        string blobName, 
        string[] subFolders,
        CancellationToken cancellationToken = default);

    
    /// <summary>
    /// Lista los blobs en un contenedor de Azure Blob Storage.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor del cual se listarán los blobs.</param>
    /// <param name="prefix">Prefijo opcional para filtrar los blobs listados.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación.</param>
    /// <returns>
    /// Una lista de nombres de blobs en el contenedor.
    /// </returns>
    
    public Task<List<string>> ListBlobsAsync(string containerName, string? prefix = null,CancellationToken cancellationToken = default);

    
    /// <summary>
    /// Obtiene la URL de un blob en Azure Blob Storage.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor que contiene el blob.</param>
    /// <param name="blobName">El nombre del blob.</param>
    /// <param name="subFolders">Sub-carpetas opcionales dentro del contenedor.</param>
    /// <returns>
    /// La URL del blob.
    /// </returns>
    
    public string GetBlobUrl(
        string containerName, 
        string blobName, 
        string[] subFolders);

    
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

    public Task CopyBlobAsync(
        string sourceContainerName,
        string sourceBlobName,
        string destinationContainerName,
        string destinationBlobName,
        string[] sourceSubFolders,
        string[] destinationSubFolders,
        CancellationToken cancellationToken = default);

    
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

    public Task MoveBlobAsync(
        string sourceContainerName,
        string sourceBlobName,
        string destinationContainerName,
        string destinationBlobName,
        string[] sourceSubFolders, 
        string[] destinationSubFolders,
        CancellationToken cancellationToken = default);
}