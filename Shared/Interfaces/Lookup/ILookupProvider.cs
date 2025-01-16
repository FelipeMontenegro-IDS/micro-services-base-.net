namespace Shared.Interfaces.Lookup;

public interface ILookupProvider<TEnum, TValue> where TEnum : Enum
{
    TValue GetValue(TEnum key, TValue defaultValue);
    TEnum GetKey(TValue value, TEnum defaultValue);
    IEnumerable<TValue> GetAllValues();
}