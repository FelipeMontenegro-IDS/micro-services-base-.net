using Shared.Bases.Lookup;
using Shared.Constants.EntityFramework;
using Shared.Enums.EntityFramework;
using Shared.Interfaces.EntityFramework;
using Shared.Interfaces.Helpers;

namespace Shared.Providers.EntityFramework;

public class SchemaProvider : BaseLookupProvider<Schema, string>, ISchemaProvider
{
    public SchemaProvider(IValidationHelper validationHelper) : base(new Dictionary<Schema, string>
    {
        { Schema.Dbo, SchemaConstant.Dbo }
    }, validationHelper)
    {
    }
}