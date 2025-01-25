using Application.Interfaces.Ardalis;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Shared.DTOs.Responses.Generals;
using Shared.Helpers;
using Shared.Interfaces.Helpers;

namespace Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ResponseDto<int>>
{
    private readonly IWriteRepositoryAsync<Customer> _repository;
    private readonly IMapper _mapper;
    private readonly IValidationHelper _validationHelper;
    // private readonly IConfigurationMicroServices _configurationMicroServices;
    // private readonly IMessageReceiver _messageReceiver;
    // private readonly IMessageSender  _messageSender;
    public CreateCustomerCommandHandler(IWriteRepositoryAsync<Customer> repository, IMapper mapper, IValidationHelper validationHelper)
    {
        _repository = repository;
        _mapper = mapper;
        _validationHelper = validationHelper;
        // _configurationMicroServices = configurationMicroServices;
        // _messageSender = messageSender;
        // _messageService = messageService; 
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
            
            // await _messageSender.SendMessageAsync(new ObjectTestRequest(){ PersonId = Guid.NewGuid() },"req_prueba_request",cancellationToken);
            // var res = await _messageService.ProcessRequestAsync<ObjectTestRequest, ObjectTestResponse>(
            //     new ObjectTestRequest { PersonId = Guid.NewGuid() },
            //     QueueRequestConstants.REQ_CONFIGURATION_BLOB_STORAGE,
            //     QueueResponseConstants.RES_CONFIGURATION_BLOB_STORAGE,
            //     new ServiceBusProcessorOptions { AutoCompleteMessages = false },
            //     cancellationToken);  
            List<string> persons = new List<string>();
            
            var data = await _repository.AddAsync(newCustomer, cancellationToken);

            if (_validationHelper.IsNull(data)) return null!;

            return new ResponseDto<int>(1, "correct.");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}