using Shared.Interfaces.Helpers;

namespace Shared.Bases.Lookup;

public class BaseLookupProvider<TEnum, TValue> : Interfaces.Lookup.ILookupProvider<TEnum, TValue> where TEnum : Enum
{
    private readonly IValidationHelper _validationHelper;
    protected readonly Dictionary<TEnum, TValue> DataLookupProviders;

    protected BaseLookupProvider(Dictionary<TEnum, TValue> dataLookupProviders, IValidationHelper validationHelper)
    {
        DataLookupProviders = dataLookupProviders;
        _validationHelper = validationHelper;
    }

    public TValue GetValue(TEnum key, TValue defaultValue)
    {
        return DataLookupProviders.GetValueOrDefault(key, defaultValue);
    }

    public TEnum GetKey(TValue value, TEnum defaultValue)
    {
        var mapping = DataLookupProviders.FirstOrDefault(kv => _validationHelper.IsNotNull(kv.Value) && kv.Value.Equals(value));
        return mapping.Key.Equals(default(TEnum)) ? defaultValue : mapping.Key;
    }

    public IEnumerable<TValue> GetAllValues()
    {
        return DataLookupProviders.Values;
    }
}