namespace Shared.Interfaces.Lookup;

public interface ILookupProvider<TEnum, TValue> where TEnum : notnull where TValue : notnull
{
    public TValue GetValue(TEnum key);
    TValue GetValue(TEnum key, TValue defaultValue);
    public TEnum GetKey(TValue value);
    TEnum GetKey(TValue value, TEnum defaultValue);
    IEnumerable<TValue> GetAllValues();
}