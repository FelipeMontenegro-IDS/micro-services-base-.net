using MediatR;

namespace Application.Features.Customers.Queries.DecryptText;

public class DecryptTextQuery : IRequest<string>
{
    public string CipherTextDecrypt;

    public DecryptTextQuery(string cipherTextDecrypt)
    {
        CipherTextDecrypt = cipherTextDecrypt;
    }
}