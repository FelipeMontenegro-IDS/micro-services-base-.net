using MediatR;

namespace Application.Features.Customers.Queries.EncryptText;

public class EncryptTextQuery : IRequest<string>
{
    public string TextToEncrypt { get; set; }

    public EncryptTextQuery(string textToEncrypt)
    {
        TextToEncrypt = textToEncrypt;
    }
}