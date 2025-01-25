using Shared.Bases.Lookup;
using Shared.Configurations;
using Shared.Enums.EntityFramework;
using Shared.Interfaces.EntityFramework;
using Shared.Interfaces.Helpers;

namespace Shared.Providers.EntityFramework;

public class EntityMetadataProvider : BaseLookupProvider<Entity, EntityMetadata>, IEntityMetadataProvider
{
    private readonly ITableProvider _tableProvider;
    private readonly ISchemaProvider _schemaProvider;

    public EntityMetadataProvider(
        IValidationHelper validationHelper,
        ITableProvider tableProvider,
        ISchemaProvider schemaProvider)
        : base(new Dictionary<Entity, EntityMetadata>
        {
            { Entity.Customers, new EntityMetadata { TableName = Table.Customers, Schema = Schema.Dbo } }
        }, validationHelper)
    {
        _tableProvider = tableProvider;
        _schemaProvider = schemaProvider;
    }

    public (string Table, string Schema) GetTableAndSchema(Entity entity)
    {
        EntityMetadata entityMetadata = GetValue(entity);
        string table =  _tableProvider.GetValue(entityMetadata.TableName);
        string schema = _schemaProvider.GetValue(entityMetadata.Schema);

        return (table, schema);
    }
}