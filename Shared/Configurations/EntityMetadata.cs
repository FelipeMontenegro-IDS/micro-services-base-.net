using Shared.Constants.EntityFramework;
using Shared.Enums.EntityFramework;

namespace Shared.Configurations;

public class EntityMetadata
{
    public Table TableName { get; set; }
    public Schema Schema { get; set; } = Schema.Dbo; // Valor  predeterminado
}