using Application.Interfaces;
using Application.Interfaces.common;
using Application.Interfaces.Microservices;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Shared.DTOs.Responses.Generals;

namespace Application.Features.customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ResponseDto<int>>
{
    private readonly IWriteRepositoryAsync<Customer> _repository;
    private readonly IMapper _mapper;
    private readonly IConfigurationMicroServices _configurationMicroServices;

    public CreateCustomerCommandHandler(IWriteRepositoryAsync<Customer> repository, IMapper mapper,
        IConfigurationMicroServices configurationMicroServices)
    {
        _repository = repository;
        _mapper = mapper;
        _configurationMicroServices = configurationMicroServices;
    }

    public async Task<ResponseDto<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var newCustomer = _mapper.Map<Customer>(request);
            var person =
                await _configurationMicroServices.GetConfigurationBlobStorageByCustomerId(
                    new ObjectTestRequest { PersonId = Guid.NewGuid() }, cancellationToken);

            var data = await _repository.AddAsync(newCustomer, cancellationToken);

            if (data == null) return null!;

            return new ResponseDto<int>(1, "correct.");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}