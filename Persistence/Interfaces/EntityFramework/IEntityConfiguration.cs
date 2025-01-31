using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Interfaces.EntityFramework;

public interface IEntityConfiguration<T> where T : class
{
    void ConfigureTable(EntityTypeBuilder<T> builder);
    void ConfigureDataEstructure(EntityTypeBuilder<T> builder);
    void ConfigureRelationships(EntityTypeBuilder<T> builder);
}