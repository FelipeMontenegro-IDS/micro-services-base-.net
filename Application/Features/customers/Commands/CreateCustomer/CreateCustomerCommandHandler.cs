﻿using Application.Interfaces;
using Application.Interfaces.common;
using Application.Interfaces.Messaging;
using Application.Interfaces.Microservices;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using Domain.Entities;
using MediatR;
using Shared.DTOs.Responses.Generals;
using Shared.Queues.Configurations;
using Shared.Utils.Helpers;

namespace Application.Features.customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ResponseDto<int>>
{
    private readonly IWriteRepositoryAsync<Customer> _repository;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    // private readonly IConfigurationMicroServices _configurationMicroServices;
    // private readonly IMessageReceiver _messageReceiver;
    private readonly IMessageSender _messageSender;
    public CreateCustomerCommandHandler(IWriteRepositoryAsync<Customer> repository, IMapper mapper,IMessageService messageService,IMessageSender messageSender)
    {
        _repository = repository;
        _mapper = mapper;
        // _configurationMicroServices = configurationMicroServices;
        _messageSender = messageSender;
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
            
            await _messageSender.SendMessageAsync(new ObjectTestRequest{ PersonId = Guid.NewGuid() },"req_prueba_request",cancellationToken);

            // var res = await _messageService.ProcessRequestAsync<ObjectTestRequest, ObjectTestResponse>(
            //     new ObjectTestRequest { PersonId = Guid.NewGuid() },
            //     QueueRequestConstants.REQ_CONFIGURATION_BLOB_STORAGE,
            //     QueueResponseConstants.RES_CONFIGURATION_BLOB_STORAGE,
            //     new ServiceBusProcessorOptions { AutoCompleteMessages = false },
            //     cancellationToken);
            
            var data = await _repository.AddAsync(newCustomer, cancellationToken);

            if (ValidationHelper.IsNull(data)) return null!;

            return new ResponseDto<int>(1, "correct.");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}