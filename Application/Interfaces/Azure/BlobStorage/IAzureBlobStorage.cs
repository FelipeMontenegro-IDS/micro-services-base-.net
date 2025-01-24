using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Shared.Enums.Data;
using Shared.Enums.Time;

namespace Application.Interfaces.Azure.BlobStorage;

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
        string[]? subFolders = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sube un archivo a Azure Blob Storage a partir de un string en formato Base64.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor donde se almacenará el blob.</param>
    /// <param name="blobName">El nombre del blob a crear.</param>
    /// <param name="base64String">El contenido del archivo en formato Base64.</param>
    /// <param name="subFolders">
    /// (Opcional) Una lista de subcarpetas donde se organizará el blob dentro del contenedor.
    /// </param>
    /// <param name="metadata">
    /// (Opcional) Metadatos adicionales que se asociarán al blob.
    /// </param>
    /// <param name="cancellationToken">Un token para cancelar la operación.</param>
    /// <returns>Una tarea que se completa cuando el archivo se ha subido exitosamente.</returns>
    public Task UploadFileFromBase64Async(
        string containerName,
        string blobName,
        string base64String,
        string[]? subFolders = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sube un archivo a Azure Blob Storage en bloques, utilizando un flujo de datos (Stream).
    /// </summary>
    /// <param name="containerName">El nombre del contenedor donde se almacenará el blob.</param>
    /// <param name="blobName">El nombre del blob a crear.</param>
    /// <param name="fileStream">El flujo del archivo que se subirá.</param>
    /// <param name="fileSize">
    /// (Opcional) El tamaño máximo de cada bloque. Por defecto, es de 50 MB.
    /// </param>
    /// <param name="subFolders">
    /// (Opcional) Una lista de sub-carpetas donde se organizará el blob dentro del contenedor.
    /// </param>
    /// <param name="metadata">
    /// (Opcional) Metadatos adicionales que se asociarán al blob.
    /// </param>
    /// <param name="cancellationToken">Un token para cancelar la operación.</param>
    /// <returns>Una tarea que se completa cuando el archivo se ha subido exitosamente.</returns>
    public Task UploadFileInBlocksAsync(
        string containerName,
        string blobName,
        Stream fileStream,
        FileSize fileSize = FileSize.Mb50,
        string[]? subFolders = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sube un archivo a Azure Blob Storage en bloques, a partir de un string en formato Base64.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor donde se almacenará el blob.</param>
    /// <param name="blobName">El nombre del blob a crear.</param>
    /// <param name="base64String">El contenido del archivo en formato Base64.</param>
    /// <param name="fileSize">
    /// (Opcional) El tamaño máximo de cada bloque. Por defecto, es de 50 MB.
    /// </param>
    /// <param name="subFolders">
    /// (Opcional) Una lista de subcarpetas donde se organizará el blob dentro del contenedor.
    /// </param>
    /// <param name="metadata">
    /// (Opcional) Metadatos adicionales que se asociarán al blob.
    /// </param>
    /// <param name="cancellationToken">Un token para cancelar la operación.</param>
    /// <returns>Una tarea que se completa cuando el archivo se ha subido exitosamente.</returns>
    public Task UploadFileInBlocksFromBase64Async(
        string containerName,
        string blobName,
        string base64String,
        FileSize fileSize = FileSize.Mb50,
        string[]? subFolders = null,
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
    /// Un flujo que representa el archivo descargado y el content-type del mismo.
    /// </returns>
    public Task<(Stream Content, string ContentType)> DownloadFileAsync(
        string containerName,
        string blobName,
        string[]? subFolders = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Descarga un blob desde Azure Blob Storage como un flujo de datos (Stream).
    /// </summary>
    /// <param name="containerName">El nombre del contenedor donde se encuentra el blob.</param>
    /// <param name="blobName">El nombre del blob a descargar.</param>
    /// <param name="subFolders">
    /// (Opcional) Una lista de subcarpetas donde se encuentra el blob dentro del contenedor.
    /// </param>
    /// <param name="cancellationToken">Un token para cancelar la operación.</param>
    /// <returns>
    /// Una tarea que devuelve una tupla que contiene el contenido del blob como un Stream 
    /// y su tipo de contenido MIME.
    /// </returns>
    public Task<(Stream Content, string ContentType)> DownloadFileBlockAsync(
        string containerName,
        string blobName,
        string[]? subFolders = null,
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
        string[]? subFolders = null,
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
    public Task DeleteFileAsync(
        string containerName,
        string blobName,
        string[]? subFolders = null,
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
    public Task<List<string>> ListBlobsAsync(
        string containerName,
        string? prefix = null,
        CancellationToken cancellationToken = default);


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
        string[]? subFolders = null);

    /// <summary>
    /// Genera la URL pública de un blob en Azure Blob Storage.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor donde se encuentra el blob.</param>
    /// <param name="blobName">El nombre del blob.</param>
    /// <param name="subFolders">
    /// (Opcional) Una lista de subcarpetas donde se encuentra el blob dentro del contenedor.
    /// </param>
    /// <returns>La URL pública del blob.</returns>
    public string GetBlobUrlBase64(
        string containerName,
        string blobName,
        string[]? subFolders = null);
    
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
        string[]? sourceSubFolders = null,
        string[]? destinationSubFolders = null,
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
        string[]? sourceSubFolders = null,
        string[]? destinationSubFolders = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Genera una cadena SAS para un blob específico, proporcionando acceso temporal.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor donde se encuentra el blob.</param>
    /// <param name="blobName">El nombre del blob.</param>
    /// <param name="expiryDuration">La duración de la validez del SAS.</param>
    /// <param name="permissions">Los permisos otorgados por la cadena SAS.</param>
    /// <param name="subFolders">
    /// (Opcional) Una lista de sub-carpetas donde se encuentra el blob dentro del contenedor.
    /// </param>
    /// <param name="sasProtocol">
    /// (Opcional) El protocolo permitido (HTTP o HTTPS). Por defecto, HTTPS.
    /// </param>
    /// <param name="timeZone">
    /// (Opcional) La zona horaria para la expiración del SAS. Por defecto, UTC.
    /// </param>
    /// <returns>La cadena SAS generada.</returns>
    public string GenerateBlobSas(
        string containerName,
        string blobName,
        TimeSpan expiryDuration,
        BlobSasPermissions permissions,
        string[]? subFolders = null,
        SasProtocol sasProtocol = SasProtocol.Https,
        TimeZoneOption timeZone = TimeZoneOption.Utc);

    /// <summary>
    /// Genera una cadena SAS para un contenedor completo, proporcionando acceso temporal.
    /// </summary>
    /// <param name="containerName">El nombre del contenedor.</param>
    /// <param name="expiryDuration">La duración de la validez del SAS.</param>
    /// <param name="permissions">Los permisos otorgados por la cadena SAS.</param>
    /// <param name="sasProtocol">
    /// (Opcional) El protocolo permitido (HTTP o HTTPS). Por defecto, HTTPS.
    /// </param>
    /// <param name="timeZone">
    /// (Opcional) La zona horaria para la expiración del SAS. Por defecto, UTC.
    /// </param>
    /// <returns>La cadena SAS generada.</returns>
    public string GenerateContainerSas(
        string containerName,
        TimeSpan expiryDuration,
        BlobContainerSasPermissions permissions,
        SasProtocol sasProtocol = SasProtocol.Https,
        TimeZoneOption timeZone = TimeZoneOption.Utc);
}