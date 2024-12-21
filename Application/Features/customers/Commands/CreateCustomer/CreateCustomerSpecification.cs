using System.Linq.Expressions;
using Application.Wrappers.Bases;
using Domain.Entities;

namespace Application.Features.customers.Commands.CreateCustomer;

public class CreateCustomerSpecification : BaseSpecification<Customer>
{
    public CreateCustomerSpecification(int age)
        : base(
            criteria: x => x.Address != "",
            orderBy: x => x.Birthdate,
            null,
            new List<Expression<Func<Customer, object>>>
            {
                x => x.Address
            })
    {}
}