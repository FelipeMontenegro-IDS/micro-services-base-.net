using Shared.Bases.LookupProvider;
using Shared.Constants;
using Shared.Enums;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

/// <summary>
/// Proporciona métodos para gestionar y obtener valores de disposición de contenido.
/// </summary>
/// <remarks>
/// La clase <see cref="ContentDispositionProvider"/> hereda de <see cref="BaseLookupProvider{TKey, TValue}"/>
/// y se encarga de mapear los tipos de disposición de contenido a sus representaciones en cadena.
/// Utiliza un diccionario para asociar cada valor del enum <see cref="ContentDisposition"/> con su
/// correspondiente constante de disposición de contenido.
/// </remarks>
public class ContentDispositionProvider : BaseLookupProvider<ContentDisposition, string>, IContentDispositionProvider
{
    public ContentDispositionProvider(IValidationHelper validationHelper) : base(
        new Dictionary<ContentDisposition, string>
        {
            { ContentDisposition.Inline, ContentDispositionConstants.Inline },
            { ContentDisposition.Attachment, ContentDispositionConstants.Attachment }
        }, validationHelper)
    {
    }

    /// <summary>
    /// Obtiene el valor de disposición de contenido formateado con el nombre de archivo.
    /// </summary>
    /// <param name="contentDisposition">El tipo de disposición de contenido.</param>
    /// <param name="fileName">El nombre del archivo a incluir en la disposición.</param>
    /// <returns>
    /// Un string que representa el encabezado de disposición de contenido, incluyendo el nombre del archivo.
    /// </returns>
    public string GetValueContentDisposition(ContentDisposition contentDisposition, string fileName)
    {
        string value = GetValue(contentDisposition, ContentDispositionConstants.Attachment);
        return value switch
        {
            ContentDispositionConstants.Inline => $"{value}; filename=\"{fileName}\"",
            ContentDispositionConstants.Attachment => $"{value}; filename=\"{fileName}\"",
            _ => value
        };
    }
}