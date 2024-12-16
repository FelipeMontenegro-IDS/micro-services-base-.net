using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey( x => x.CustomerId);
        builder.Property(x => x.Name).IsRequired(false);
        builder.Property(x => x.Birthdate);
        builder.Property(x => x.Telefono).IsRequired(false);
        builder.Property(x => x.Email).IsRequired(false);
    }
}