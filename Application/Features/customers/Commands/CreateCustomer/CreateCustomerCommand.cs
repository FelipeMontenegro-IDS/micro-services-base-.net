using Application.Wrappers.Responses;
using MediatR;

namespace Application.Features.customers.Commands.CreateCustomer;

public class CreateCustomerCommand : IRequest<Response<int>>
{
    public string Name { get; set; }
    public DateTime? Birthdate { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}