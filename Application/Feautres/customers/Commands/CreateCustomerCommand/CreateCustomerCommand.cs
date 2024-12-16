using Application.Wrappers.responses;
using MediatR;

namespace Application.Feautres.customers.Commands.CreateCustomerCommand;

public class CreateCustomerCommand : IRequest<Response<int>>
{
    
    public string Name { get; set; }
    public DateTime Birthdate { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}