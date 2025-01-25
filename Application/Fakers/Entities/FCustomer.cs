using Bogus;
using Domain.Entities;

namespace Application.Fakers.Entities;

public static class FCustomer
{
    public static Faker<Customer> CreateFaker()
    {
        return new Faker<Customer>("es")
            .RuleFor(x => x.CustomerId, f => f.Random.Guid())
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email)
            .RuleFor(x => x.Address, f => f.Address.StreetAddress())
            .RuleFor(x => x.Telefono,f => f.Phone.PhoneNumber())
            .RuleFor(x => x.Birthdate, f => f.Date.Past(21));
    }
}