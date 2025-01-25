using Shared.Interfaces.Helpers;
using Shared.Interfaces.Lookup;

namespace Shared.Bases.Lookup;

public class BaseLookupProvider<TEnum, TValue> : ILookupProvider<TEnum, TValue>
    where TEnum : notnull where TValue : notnull
{
    private readonly IValidationHelper _validationHelper;
    protected readonly Dictionary<TEnum, TValue> DataLookupProviders;

    protected BaseLookupProvider(Dictionary<TEnum, TValue> dataLookupProviders, IValidationHelper validationHelper)
    {
        DataLookupProviders = dataLookupProviders;
        _validationHelper = validationHelper;
    }

    public TValue GetValue(TEnum key)
    {
        if (!DataLookupProviders.TryGetValue(key, out var value))
        {
            throw new KeyNotFoundException($"No value found for key '{key}'.");
        }

        return value;
    }

    public TValue GetValue(TEnum key, TValue defaultValue)
    {
        return DataLookupProviders.GetValueOrDefault(key, defaultValue);
    }

    public TEnum GetKey(TValue value)
    {
        KeyValuePair<TEnum, TValue> mapping = DataLookupProviders
            .FirstOrDefault(kv => _validationHelper.IsNotNull(kv.Value) && kv.Value.Equals(value));

        if (mapping.Equals(default(KeyValuePair<TEnum, TValue>)))
        {
            throw new KeyNotFoundException($"No key found for value '{value}'.");
        }

        return mapping.Key;
    }

    public TEnum GetKey(TValue value, TEnum defaultValue)
    {
        KeyValuePair<TEnum, TValue> mapping =
            DataLookupProviders.FirstOrDefault(kv => _validationHelper.IsNotNull(kv.Value) && kv.Value.Equals(value));
        return mapping.Key.Equals(default(TEnum)) ? defaultValue : mapping.Key;
    }

    public IEnumerable<TValue> GetAllValues()
    {
        return DataLookupProviders.Values;
    }

    public bool ContainsValue(TValue value)
    {
        return DataLookupProviders.Values.Contains(value);
    }

    public bool ContainsKey(TEnum key)
    {
        return DataLookupProviders.ContainsKey(key);
    }
}