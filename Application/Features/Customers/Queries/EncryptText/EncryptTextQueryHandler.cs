using MediatR;
using Shared.Interfaces.Providers.Data;

namespace Application.Features.Customers.Queries.EncryptText;

public class EncryptTextQueryHandler : IRequestHandler<EncryptTextQuery, string>
{
    private readonly ISecretKeyProvider _secretKeyProvider;

    public EncryptTextQueryHandler(ISecretKeyProvider secretKeyProvider)
    {
        _secretKeyProvider = secretKeyProvider;
    }

    public Task<string> Handle(EncryptTextQuery request, CancellationToken cancellationToken)
    {
        var textEncrypt = _secretKeyProvider.EncryptJwt(request.TextToEncrypt);
        return Task.FromResult(textEncrypt);
    }
}