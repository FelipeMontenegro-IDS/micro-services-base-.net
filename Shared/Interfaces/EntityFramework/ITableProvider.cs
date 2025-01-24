using Shared.Enums.EntityFramework;
using Shared.Interfaces.Lookup;

namespace Shared.Interfaces.EntityFramework;

public interface ITableProvider : ILookupProvider<Table,string>
{
    
}