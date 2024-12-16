using Application.Interfaces;
using Application.Wrappers.responses;
using Domain.Entities;
using MediatR;

namespace Application.Features.customers.Commands.CreateCustomerCommand;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand,Response<int>>
{
    // private readonly IReadRepositoryAsync<Customer> _repository;
    public CreateCustomerCommandHandler(/*IReadRepositoryAsync<Customer> repository*/)
    {
        // _repository = repository;
    }

    public async  Task<Response<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // var person = new List<int>();

        // CreateCustomerSpecification specification = new CreateCustomerSpecification(1);
        // var response = await _repository.FirstOrDefaultAsync(specification);
        throw new NotImplementedException() ; 
    }
}