using Shared.Interfaces.Helpers;

namespace Shared.Bases.Lookup;

public class BaseLookupProvider<TEnum, TValue> : Interfaces.Lookup.ILookupProvider<TEnum, TValue> where TEnum : Enum
{
    private readonly IValidationHelper _validationHelper;
    private readonly Dictionary<TEnum, TValue> _dataLookupProviders;

    protected BaseLookupProvider(Dictionary<TEnum, TValue> dataLookupProviders, IValidationHelper validationHelper)
    {
        _dataLookupProviders = dataLookupProviders;
        _validationHelper = validationHelper;
    }

    public TValue GetValue(TEnum key, TValue defaultValue)
    {
        return _dataLookupProviders.GetValueOrDefault(key, defaultValue);
    }

    public TEnum GetKey(TValue value, TEnum defaultValue)
    {
        var mapping = _dataLookupProviders.FirstOrDefault(kv => _validationHelper.IsNotNull(kv.Value) && kv.Value.Equals(value));
        return mapping.Key.Equals(default(TEnum)) ? defaultValue : mapping.Key;
    }

    public IEnumerable<TValue> GetAllValues()
    {
        return _dataLookupProviders.Values;
    }
}