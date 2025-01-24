using Shared.Bases.Lookup;
using Shared.Constants.EntityFramework;
using Shared.Enums.EntityFramework;
using Shared.Interfaces.EntityFramework;
using Shared.Interfaces.Helpers;

namespace Shared.Providers.EntityFramework;

public class TableProvider : BaseLookupProvider<Table, string>, ITableProvider
{
    public TableProvider(IValidationHelper validationHelper)
        : base(new Dictionary<Table, string>
        {
            { Table.Customers, TableConstant.Customers }
        }, validationHelper)
    {
    }
    
}