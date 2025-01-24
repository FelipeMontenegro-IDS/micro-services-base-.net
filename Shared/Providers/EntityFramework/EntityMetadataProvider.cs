using Shared.Bases.Lookup;
using Shared.Configurations;
using Shared.Enums.EntityFramework;
using Shared.Interfaces.EntityFramework;
using Shared.Interfaces.Helpers;

namespace Shared.Providers.EntityFramework;

public class EntityMetadataProvider : BaseLookupProvider<Entity, EntityMetadata>, IEntityMetadataProvider
{
    public EntityMetadataProvider(IValidationHelper validationHelper)
        : base(new Dictionary<Entity, EntityMetadata>
            {
                { Entity.Customers , new EntityMetadata { TableName = Table.Customers , Schema = Schema.Dbo } }
            }, validationHelper)
    {
    }
}