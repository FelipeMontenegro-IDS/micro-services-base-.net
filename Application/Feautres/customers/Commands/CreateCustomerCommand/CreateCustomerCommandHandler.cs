using Application.Wrappers.responses;
using MediatR;

namespace Application.Feautres.customers.Commands.CreateCustomerCommand;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand,Response<int>>
{
    public CreateCustomerCommandHandler()
    {
    }

    public async  Task<Response<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var person = new List<int>();
        throw new NotImplementedException(); 
    }
}