using Application.Fakers.Entities;
using Application.Interfaces.Faker;
using Domain.Entities;

namespace Application.Fakers.Generators;

public class FCustomerGenerator : IFaker<Customer>
{
    public Customer Generate()
    {
        return FCustomer.CreateFaker().Generate();
    }

    public IEnumerable<Customer> Generate(int count)
    {
        return FCustomer.CreateFaker().Generate(count);
    }
}