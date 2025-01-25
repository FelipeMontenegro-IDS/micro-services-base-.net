using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Configurations;
using Shared.Enums.EntityFramework;
using Shared.Interfaces.EntityFramework;
using Shared.Interfaces.Helpers;
using Shared.Providers.EntityFramework;

namespace Persistence.Configurations;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    private readonly IEntityMetadataProvider _entityMetadataProvider;
    public CustomerConfig(IEntityMetadataProvider entityMetadataProvider)
    {
        _entityMetadataProvider = entityMetadataProvider;
    }
    
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        var (table,schema) = _entityMetadataProvider.GetTableAndSchema(Entity.Customers);
        
        builder.ToTable(table, schema);
        builder.HasKey(x => x.CustomerId);
        builder.Property(x => x.Name).IsRequired(false);
        builder.Property(x => x.Birthdate);
        builder.Property(x => x.Telefono).IsRequired(false);
        builder.Property(x => x.Email).IsRequired(false);
    }
}