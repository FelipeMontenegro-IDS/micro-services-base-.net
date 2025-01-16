using Application.Interfaces.Azure.BlobStorage;
using Application.Interfaces.Common;
using Azure.Storage.Sas;
using MediatR;
using Shared.Interfaces.Helpers;

namespace Application.Features.Customers.Queries.GenerateUrlTemporal;

public class GenerateUrlTemporalQueryHandler : IRequestHandler<GenerateUrlTemporalQuery,UrlTemporal>
{
    private readonly IAzureBlobStorage _azureBlobStorage;
    private readonly ITimeSpanHelper _timeSpanHelper;
    
    public GenerateUrlTemporalQueryHandler(IAzureBlobStorage azureBlobStorage,ITimeSpanHelper timeSpanHelper)
    {
        _azureBlobStorage = azureBlobStorage;
        _timeSpanHelper = timeSpanHelper;
    }
    
    public Task<UrlTemporal> Handle(GenerateUrlTemporalQuery request, CancellationToken cancellationToken)
    {
        UrlTemporal urlTemporal = new UrlTemporal();
        
        var url = _azureBlobStorage.GenerateBlobSas("images","img.jpg",_timeSpanHelper.CreateTimeSpanFromMinutes(2),BlobSasPermissions.Read);
        
        urlTemporal.Url = url;
        
        return Task.FromResult(urlTemporal);
        
    }
}