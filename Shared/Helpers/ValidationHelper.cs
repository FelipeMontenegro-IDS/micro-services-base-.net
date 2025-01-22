using System.Diagnostics.CodeAnalysis;
using Shared.Interfaces.Helpers;

namespace Shared.Helpers;

public class ValidationHelper : IValidationHelper
{
    public bool IsNull<T>(T? value)
    {
        return value == null;
    }

    public bool IsNull<T>(T? value) where T : struct
    {
        return !value.HasValue;
    }

    public bool IsNotNull<T>([NotNullWhen(true)] T? value)
    {
        return value != null;
    }

    public bool IsNotNull<T>([NotNullWhen(true)] T? value) where T : struct
    {
        return value.HasValue;
    }

    public bool IsValidEnum<T>(T? value) where T : struct, Enum
    {
        return IsNotNull(value) && Enum.IsDefined(typeof(T), value);
    }

    public bool HasValues<T>(IEnumerable<T> collection)
    {
        if (IsNull(collection)) return false;
        using var enumerator = collection.GetEnumerator();
        return enumerator.MoveNext();
    }
}