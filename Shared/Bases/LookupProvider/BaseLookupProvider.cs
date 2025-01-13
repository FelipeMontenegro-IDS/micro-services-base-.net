using Shared.Helpers;
using Shared.Interfaces.LookupProvider;

namespace Shared.Bases.LookupProvider;

public class BaseLookupProvider<TEnum, TValue> : ILookupProvider<TEnum, TValue> where TEnum : Enum
{
    protected readonly Dictionary<TEnum, TValue> DataLookupProviders;

    protected BaseLookupProvider(Dictionary<TEnum, TValue> dataLookupProviders)
    {
        DataLookupProviders = dataLookupProviders;
    }

    public TValue GetValue(TEnum key, TValue defaultValue)
    {
        return DataLookupProviders.GetValueOrDefault(key, defaultValue);
    }

    public TEnum GetKey(TValue value, TEnum defaultValue)
    {
        var mapping =
            DataLookupProviders.FirstOrDefault(kv => ValidationHelper.IsNotNull(kv.Value) && kv.Value.Equals(value));

        if (ValidationHelper.IsNotNull<TEnum>(mapping.Key) && ValidationHelper.IsNotNull<TValue>(mapping.Value))
        {
            return mapping.Key;
        }

        return defaultValue;
    }

    public IEnumerable<TValue> GetAllValues()
    {
        return DataLookupProviders.Values;
    }
}