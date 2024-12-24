using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Shared.DTOs.Responses.Generals;

namespace Application.Features.customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ResponseDto<int>>
{
    private readonly IWriteRepositoryAsync<Customer> _repository;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(IWriteRepositoryAsync<Customer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseDto<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var newCustomer = _mapper.Map<Customer>(request);
        var data = await _repository.AddAsync(newCustomer, cancellationToken);
        if (data == null) return null!;
        return new ResponseDto<int>(1, "correct.");
    }
}