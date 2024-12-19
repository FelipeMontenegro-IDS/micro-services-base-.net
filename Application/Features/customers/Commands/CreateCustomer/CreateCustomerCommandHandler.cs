using Application.Interfaces;
using Application.Wrappers.responses;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Response<int>>
{
    private readonly IWriteRepositoryAsync<Customer> _repository;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(IWriteRepositoryAsync<Customer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var newCustomer = _mapper.Map<Customer>(request);
        var data = await _repository.AddAsync(newCustomer, cancellationToken);
        if (data == null) return null!;
        return new Response<int>(1, "correct.");
    }
}