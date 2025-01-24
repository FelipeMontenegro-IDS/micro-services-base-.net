using Shared.Enums.EntityFramework;
using Shared.Interfaces.Lookup;

namespace Shared.Interfaces.EntityFramework;

public interface ISchemaProvider : ILookupProvider<Schema,string>
{
        
}