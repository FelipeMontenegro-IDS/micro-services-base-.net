using Application.Interfaces;
using Application.Interfaces.common;
using Application.Interfaces.Messaging;
using Application.Interfaces.Microservices;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using Domain.Entities;
using MediatR;
using Shared.DTOs.Responses.Generals;

namespace Application.Features.customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ResponseDto<int>>
{
    private readonly IWriteRepositoryAsync<Customer> _repository;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    // private readonly IConfigurationMicroServices _configurationMicroServices;
    // private readonly IMessageReceiver _messageReceiver;
    //private readonly IMessageSender _messageSender;
    public CreateCustomerCommandHandler(IWriteRepositoryAsync<Customer> repository, IMapper mapper,IMessageService messageService)
    {
        _repository = repository;
        _mapper = mapper;
        // _configurationMicroServices = configurationMicroServices;
        _messageService = messageService; 
    }

    public async Task<ResponseDto<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var newCustomer = _mapper.Map<Customer>(request);

            // var person = _messageReceiver.RegisterMessageHandler<ObjectTestResponse>("res_configuration_blob_storage", async (message,token) =>
            // {
            //     Console.WriteLine(message);
            // },new ServiceBusProcessorOptions{ AutoCompleteMessages = false},cancellationToken);
            
            // await _messageSender.SendMessageAsync(new ObjectTestRequest{ PersonId = Guid.NewGuid() },"req_prueba_request",cancellationToken);

            await _messageService.SendAsync(new ObjectTestRequest{ PersonId = Guid.NewGuid() },"req_prueba_request",null,cancellationToken);
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