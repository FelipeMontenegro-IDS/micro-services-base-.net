using System.Linq.Expressions;
using Application.Wrappers.common.responses;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Features.customers.Commands.CreateCustomer;

public class CreateCustomerSpecification : BaseSpecification<Customer>
{
    public CreateCustomerSpecification(int age)
        : base(
            expression: x => x.Address != "",
            orderBy: x => x.Birthdate,
            new List<Expression<Func<Customer, object>>>
            {
                x => x.Address
            })
    {
    }
}