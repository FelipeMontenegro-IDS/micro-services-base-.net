using Shared.Configurations;
using Shared.Enums.EntityFramework;
using Shared.Interfaces.Lookup;

namespace Shared.Interfaces.EntityFramework;

public interface IEntityMetadataProvider : ILookupProvider<Entity,EntityMetadata>
{
    
}