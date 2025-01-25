using System.Reflection.PortableExecutable;
using Domain.Bases;

namespace Domain.Entities;

public class Customer : BaseEntity
{
    public Guid CustomerId { get; set; }
    public string Name { get; set; }
    public DateTime Birthdate { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public Customer()
    {

    }
}
