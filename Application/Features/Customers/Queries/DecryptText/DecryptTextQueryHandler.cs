using MediatR;
using Shared.Interfaces.Providers.Data;

namespace Application.Features.Customers.Queries.DecryptText;

public class DecryptTextQueryHandler : IRequestHandler<DecryptTextQuery,string>
{
    private readonly ISecretKeyProvider _secretKeyProvider;

    public DecryptTextQueryHandler(ISecretKeyProvider secretKeyProvider)
    {
        _secretKeyProvider = secretKeyProvider;
    }
    
    public Task<string> Handle(DecryptTextQuery request, CancellationToken cancellationToken)
    {
        string text = _secretKeyProvider.DecryptJwt(request.CipherTextDecrypt);
        return Task.FromResult(text);
    }
}